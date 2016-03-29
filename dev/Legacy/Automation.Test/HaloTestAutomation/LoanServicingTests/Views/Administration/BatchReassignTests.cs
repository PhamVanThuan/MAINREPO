using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class BatchReassignTests : TestBase<WorkflowBatchReassign>
    {
        protected override void OnTestTearDown()
        {
            if (base.Browser != null)
            {
                base.Browser.Dispose();
                base.Browser = null;
            }
            base.OnTestTearDown();
        }

        [Test, Description("Batch reassigns cases for a user that plays a workflow role in case.")]
        public void BatchReassignWorkflowRoleWorkflowCases()
        {
            var genericKeys = BatchReassignCases(TestUsers.PersonalLoanSupervisor1, WorkflowRoleType.PLConsultantD, TestUsers.PersonalLoanConsultant1, 2, Workflows.PersonalLoans);
            foreach (var genericKey in genericKeys)
            {
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(genericKey, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD, TestUsers.PersonalLoanConsultant1);
                X2Assertions.AssertWorklistOwner(genericKey, string.Empty, Workflows.PersonalLoans, TestUsers.PersonalLoanConsultant1);
            }
        }

        [Test, Description("Batch reassigns cases for a user that plays an offer role in case.")]
        public void BatchReassignOfferRoleWorkflowCases()
        {
            var genericKeys = BatchReassignCases(TestUsers.NewBusinessSupervisor, OfferRoleTypes.NewBusinessProcessor, TestUsers.NewBusinessProcessor, 3, Workflows.ApplicationManagement);
            foreach (var genericKey in genericKeys)
            {
                AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(genericKey, Common.Enums.OfferRoleTypeEnum.NewBusinessProcessorD, TestUsers.NewBusinessProcessor);
                X2Assertions.AssertWorklistOwner(genericKey, string.Empty, Workflows.ApplicationManagement, TestUsers.NewBusinessProcessor);
            }
        }

        [Test, Description("Batch reassigns a group of debt counselling cases by selecting a single case in the group")]
        public void BatchReassignDebtCounsellingGroup()
        {
            var idNumber = Service<ILegalEntityService>().GetLegalEntityIDNumberPlaySameRoleTwiceDifferentAccounts(Common.Enums.RoleTypeEnum.MainApplicant, true, false);
            var accounts = Service<ILegalEntityService>().GetLegalEntityLoanAccountsByIDNumber(idNumber);
            if (accounts.Count != 2)
                Assert.Fail(string.Format(@"Test didnt find data correctly tried to use LE ID Number {0}", idNumber));
            //we can create debt counselling cases for both of these accounts
            bool created = Service<IDebtCounsellingService>().CreateDebtCounsellingCase(accounts[0].AccountKey);
            if (created)
            {
                created = Service<IDebtCounsellingService>().CreateDebtCounsellingCase(accounts[1].AccountKey);
            }
            else
            {
                Assert.Fail(string.Format(@"Could not create debt counselling case for account {0}", accounts[0].AccountKey));
            }
            if (created)
            {
                //we can continue
                var case1 = Service<IDebtCounsellingService>().GetDebtCounsellingByAccountKeyAndStatus(accounts[0].AccountKey);
                var case2 = Service<IDebtCounsellingService>().GetDebtCounsellingByAccountKeyAndStatus(accounts[1].AccountKey);
                Assert.That(case1.Rows(0).Column("DebtCounsellingGroupKey").GetValueAs<int>() == case2.Rows(0).Column("DebtCounsellingGroupKey").GetValueAs<int>());
                //now we can try and batch reassign one of them
                int debtCounsellingKey1 = case1.Rows(0).Column("DebtCounsellingKey").GetValueAs<int>();
                int debtCounsellingKey2 = case2.Rows(0).Column("DebtCounsellingKey").GetValueAs<int>();
                //get the worklist owner - should always be DCAUser
                var worklist = Service<IX2WorkflowService>().GetWorklistDetails(debtCounsellingKey1, string.Empty, Workflows.DebtCounselling).FirstOrDefault();
                //find a new person
                string newOwner = Service<IADUserService>().GetADUserPlayingWorkflowRole(WorkflowRoleType.DebtCounsellingAdminD, worklist.ADUserName);
                //reassign the case
                BatchReassignSpecificCase(TestUsers.DebtCounsellingSupervisor, WorkflowRoleType.DebtCounsellingAdminD, accounts[0].AccountKey, worklist.ADUserName, Workflows.DebtCounselling, newOwner);
                //both cases should now be assigned to the new owner
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey1, Common.Enums.WorkflowRoleTypeEnum.DebtCounsellingAdminD, newOwner);
                X2Assertions.AssertWorklistOwner(debtCounsellingKey1, string.Empty, Workflows.DebtCounselling, newOwner);
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey2, Common.Enums.WorkflowRoleTypeEnum.DebtCounsellingAdminD, newOwner);
                X2Assertions.AssertWorklistOwner(debtCounsellingKey2, string.Empty, Workflows.DebtCounselling, newOwner);
            }
        }

        private List<int> BatchReassignCases(string userToLoginAs, string role, string newUser, int noOfCases, string workflow)
        {
            base.Browser = new TestBrowser(userToLoginAs);
            //go to the batch reassign screen
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().BatchReassign(base.Browser);
            return base.Browser.Page<WorkflowBatchReassign>().BatchReassign(role, newUser, noOfCases, workflow);
        }

        private void BatchReassignSpecificCase(string userToLoginAs, string role, int key, string currentOwner, string workflow, string newOwner)
        {
            base.Browser = new TestBrowser(userToLoginAs);
            //go to the batch reassign screen
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().BatchReassign(base.Browser);
            base.Browser.Page<WorkflowBatchReassign>().BatchReassignSpecificCase(role, key, currentOwner, workflow, newOwner);
        }
    }
}