using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Automation.DataAccess;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using BuildingBlocks;

namespace Origination
{
    [TestFixture, RequiresSTA]
    public class WorkflowIntegrationTests : OriginationTestBase<BasePage>
    {
        //[Test, Sequential]
        public void BulkQACompleteCases(
            [Values(false, true)] bool isFL,
            [Values(@"SAHL\CUUser{0}", @"SAHL\FLAppProcUser{0}")] string user,
            [Values(5, 20)] int noOfCases
            )
        {
            var scriptEngine = new X2ScriptEngine();
            var tasks = new List<Task>();
            var applications = Service<IX2WorkflowService>().GetXNumberOfOffersAtQA(noOfCases, isFL);
            int userid = 0;
            foreach (var application in applications)
            {
                userid++;
                if (userid > 5)
                    userid = 1;
                var identity = string.Format(user, userid);
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAComplete", application.OfferKey, identity);
                    var failedSteps = (from r in results where r.Value.ActivityCompleted == false select r.Value.X2Messages);
                    foreach (var m in failedSteps)
                    {
                        Debug.WriteLine(m.ToString());
                    }
                }
                ));
            }
            Task.WaitAll(tasks.ToArray());
            string endState = isFL ? WorkflowStates.ApplicationManagementWF.ManageApplication : WorkflowStates.ApplicationManagementWF.IssueAIP;
            foreach (var application in applications)
            {
                X2Assertions.AssertCurrentAppManX2State(application.OfferKey, endState);
            }
        }

        //[Test]
        public void BulkApplicationInOrder()
        {
            var scriptEngine = new X2ScriptEngine();
            var tasks = new List<Task>();
            var applications = Service<IX2WorkflowService>().GetXNumberOfCaseForAppInOrder(50);
            int userid = 0;
            string user = @"SAHL\NBPUser{0}";
            foreach (var application in applications)
            {
                userid++;
                if (userid > 5)
                    userid = 1;
                var identity = string.Format(user, userid);
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "ApplicationInOrder", application.OfferKey, identity);
                    var failedSteps = (from r in results where r.Value.ActivityCompleted == false select r.Value.X2Messages);
                    Assert.That(failedSteps.Any(), string.Format("No failed steps reported {0}", application.OfferKey));
                    var messages = results.Where(x => x.Value.ActivityCompleted == false).SelectMany(y => y.Value.X2Messages);
                    var enumerable = messages as IList<string> ?? messages.ToList();
                    Assert.That(enumerable.Any(), string.Format("No messages reported {0}", application.OfferKey));
                    foreach (var msg in enumerable)
                    {
                        Debug.WriteLine(string.Format("Application {0} - Messages: {1}", application.OfferKey, msg));
                        Assert.That(msg.Contains("serious internal error ") == false);
                    }
                }
                ));
            }
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void WorkflowAssignment_ReactiveUserOrRoundRobinForOSKeyByProcess_WhenADUserStatusIsActive_AndRRStatusIsInactive()
        {
            try
            {
                QueryResults results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor1);
                int offerKey = results.First().Column("Name").GetValueAs<int>();
                //we need to send a case to application query
                base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.QueryOnApplication, offerKey);
                //update our New Business Processor to inactive RR status
                Service<IADUserService>().UpdateADUserStatus(TestUsers.NewBusinessProcessor1, GeneralStatusEnum.Active, GeneralStatusEnum.Inactive, GeneralStatusEnum.Active);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
                //return the case back
                base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FeedbackOnQuery, offerKey);
                //should be assigned to the same NBPUser
                AssignmentAssertions.AssertWorkflowAssignment(TestUsers.NewBusinessProcessor1, offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            }
            finally
            {
                //revert user status
                Service<IADUserService>().UpdateADUserStatus(TestUsers.NewBusinessProcessor1, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
            }
        }

        [Test, Repeat(5)]
        public void WorkflowAssignment_ReactiveUserOrRoundRobinForOSKeyByProcess_WhenADUserStatusIsInActive_AndRRStatusIsInactive()
        {
            try
            {
                QueryResults results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor1);
                int offerKey = results.First().Column("Name").GetValueAs<int>();
                //we need to send a case to application query
                base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.QueryOnApplication, offerKey);
                //update our New Business Processor to inactive ADUser and inactive RR status
                Service<IADUserService>().UpdateADUserStatus(TestUsers.NewBusinessProcessor1, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
                string expectedUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.NewBusinessProcessorD, RoundRobinPointerEnum.NewBusinessProcessor);
                //return the case back
                base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FeedbackOnQuery, offerKey);
                //should be assigned to a new NBPUser
                AssignmentAssertions.AssertWorkflowAssignment(expectedUser, offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            }
            finally
            {
                //revert user status
                Service<IADUserService>().UpdateADUserStatus(TestUsers.NewBusinessProcessor1, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
            }
        }

        [Test]
        public void WorkflowAssignment_ReActivateBranchUsersForOrigination()
        {
            QueryResults results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor1);
            int offerKey = results.First().Column("Name").GetValueAs<int>();
            //get the branch consultant
            string consultantADUserName = Service<IApplicationService>().GetADUserNameOfFirstActiveOfferRole(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            string adminADUserName = Service<IApplicationService>().GetADUserNameOfFirstActiveOfferRole(offerKey, OfferRoleTypeEnum.BranchAdminD);
            //we need to send a case to application query
            base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.QueryOnApplication, offerKey);
            AssignmentAssertions.AssertWorkflowAssignment(consultantADUserName, offerKey, OfferRoleTypeEnum.BranchConsultantD);
            //there might not always be an admin assigned to a case
            if (!string.IsNullOrEmpty(adminADUserName))
                AssignmentAssertions.AssertWorkflowAssignment(adminADUserName, offerKey, OfferRoleTypeEnum.BranchAdminD);
        }

        [Test]
        public void WorkflowAssignment_DeActiveUsersForInstanceAndProcess()
        {
            QueryResults results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor1);
            int offerKey = results.First().Column("Name").GetValueAs<int>();
            //NTU Archive the case
            base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.NTU, offerKey);
            base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireNTUTimeoutTimer, offerKey);
            var instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
            AssignmentAssertions.AssertAllWorkflowAssignmentRecordsForInstanceAreInactive(instanceID);
        }

        [Test]
        public void WorkflowAssignment_GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct_When_User_Is_No_Longer_Active()
        {
            try
            {
                QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.FLProcessor3, (int)OfferTypeEnum.FurtherAdvance);
                int offerKey = results.Last().Column("OfferKey").GetValueAs<int>();
                Service<IADUserService>().UpdateADUserStatus(TestUsers.FLProcessor3, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
                string expectedUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD, RoundRobinPointerEnum.FLProcessor);
                //submit the case
                scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.FurtherLending.ApplicationInOrder, offerKey, TestUsers.FLProcessor3);
                Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.Credit);
                scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment, offerKey, TestUsers.CreditSupervisor2);
                scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ApproveApplication, offerKey, TestUsers.CreditSupervisor2);
                //case should end up at Contact Client
                Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ReadvancePayments, WorkflowStates.ReadvancePaymentsWF.ContactClient);
                AssignmentAssertions.AssertWorkflowAssignment(expectedUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            }
            finally
            {
                Service<IADUserService>().UpdateADUserStatus(TestUsers.FLProcessor3, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.Origination, Workflows.ApplicationCapture, CacheTypes.DomainService);
            }
        }

        [Test]
        public void WorkflowAssignment_GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct_When_User_Is_Still_Active()
        {
            QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.FLProcessor3, (int)OfferTypeEnum.FurtherAdvance);
            int offerKey = results.Last().Column("OfferKey").GetValueAs<int>();
            //submit the case
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.FurtherLending.ApplicationInOrder, offerKey, TestUsers.FLProcessor3);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.Credit);
            scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment, offerKey, TestUsers.CreditSupervisor2);
            scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ApproveApplication, offerKey, TestUsers.CreditSupervisor2);
            //case should end up at Contact Client
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ReadvancePayments, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            AssignmentAssertions.AssertWorkflowAssignment(TestUsers.FLProcessor3, offerKey, OfferRoleTypeEnum.FLProcessorD);
        }
        /// <summary>
        /// test
        /// </summary>
        [Ignore]
        [Test, TestCaseSource(typeof(WorkflowIntegrationTests), "GetIDSeeds")]
        public void insert_new_id_numbers(string seed)
        {
            string idNumber = null;
            for (int i = 0; i < 200; i++)
            {
                idNumber = IDNumbers.GetNextIDNumber(seed, idNumber);
                Service<ICommonService>().InsertIDNumberIntoTestTable(idNumber);
            }
        }

        public List<string> GetIDSeeds()
        {
            return new List<string> {    "800101", "800102", "800103", "800104", "800105", "800106", "800107", 
                                         "800108", "800109", "800110", "800111", "800112", "800113", "800114", 
                                         "800115", "800116", "800117", "800118", "800119", "800120", "800121",
                                         "800122", "800123", "840116", "821104", "550105", "581212", "650506",
                                         "761109", "850407"
            }; 
        }
    }
}