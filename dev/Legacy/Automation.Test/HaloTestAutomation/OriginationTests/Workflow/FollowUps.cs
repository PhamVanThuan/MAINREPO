using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace Origination.Workflow
{
    /// <summary>
    /// Contains tests for creating and continuing with Follow Ups in Application Management
    /// </summary>
    [TestFixture, RequiresSTA]
    public class FollowUps : Origination.OriginationTestBase<BasePage>
    {
        private TestBrowser _browser;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            BatchReassignCases("New Business Processor", TestUsers.NewBusinessProcessor, TestUsers.NewBusinessManager);
        }

        /// <summary>
        /// Batch reassigns cases to our new business processor
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userName"></param>
        /// <param name="loginUser"></param>
        private void BatchReassignCases(string role, string userName, string loginUser)
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            int noOfCases = 10;
            int currentCases = Service<IX2WorkflowService>().GetCountofCasesForUser(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                userName);
            if (currentCases < noOfCases)
            {
                int requiredCases = noOfCases - currentCases;
                //login as a user who can access the batch reassign
                _browser = new TestBrowser(loginUser, TestUsers.Password);
                _browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(_browser);
                _browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().BatchReassign(_browser);
                _browser.Page<WorkflowBatchReassign>().BatchReassign(role, userName, requiredCases, Workflows.ApplicationManagement);
            }
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (_browser != null)
            {
                try
                {
                    _browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    _browser.Dispose();
                    _browser = null;
                }
            }
        }

        #region Tests

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void CreateFollowUp()
        {
            int offerKey = CreateFollowUpCase();
        }

        /// <summary>
        /// Creates a case at the Follow Up Hold state
        /// </summary>
        /// <returns></returns>
        internal int CreateFollowUpCase()
        {
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor, 1,
                "Followup");
            _browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            _browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(_browser);

            //search for the case and add it to the FloBo
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            _browser.ClickAction(WorkflowActivities.ApplicationManagement.CreateFollowup);
            string CurrentMinutes = DateTime.Now.Minute.ToString();
            int minsToAdd = 5;
            Regex regex = new Regex(@"[4,9]");
            if (regex.IsMatch(CurrentMinutes))
            {
                minsToAdd = 10;
            }
            else
            {
                minsToAdd = 5;
            }

            string hourValueToAssert;
            string minValueToAssert;
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            _browser.Page<MemoFollowUpAdd>().CreateFollowup(minsToAdd, out hourValueToAssert, out minValueToAssert);
            int instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByOfferKey(offerKey);
            //Something needs to wait for the clone to be created...
            Thread.Sleep(5000);
            //Service<IX2WorkflowService>().WaitForAppManCaseCreate(offerKey);
            //assert that the case was sent to the correct state
            X2Assertions.AssertTop1X2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.FollowupHold, Workflows.ApplicationManagement);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(offerKey, ScheduledActivities.ApplicationManagement.OnFollowup, 0, Convert.ToInt32(hourValueToAssert), Convert.ToInt32(minValueToAssert), 1, true);
            //assert only one 694 and 101 OfferRole exists
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1,
                ((int)OfferRoleTypeEnum.NewBusinessProcessorD).ToString());
            return offerKey;
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void ReinstateFollowUp()
        {
            int offerKey = CreateFollowUpCase();
            //get the instance ID
            Int64 instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.FollowupHold, offerKey, true);
            //we need to update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireOnFollowupTimer, offerKey);
            //wait for timer
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.OnFollowup, instanceID, 1);
            //case should now be at the Ready to Follow Up state
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            //reinstate the follow up
            _browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateFollowup);
            //create a new follow up time
            string hourToAssert;
            string minuteToAssert;
            _browser.Page<MemoFollowUpAdd>().CreateFollowup(45, out hourToAssert, out minuteToAssert);
            //assert that the case was sent to the correct state
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.FollowupHold, Workflows.ApplicationManagement);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(offerKey, ScheduledActivities.ApplicationManagement.OnFollowup, 0, Convert.ToInt32(hourToAssert),
                Convert.ToInt32(minuteToAssert), 1, true);
            //assert only one 694 and 101 OfferRole exists
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1,
                ((int)OfferRoleTypeEnum.NewBusinessProcessorD).ToString());
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void ContinueWithFollowUpReadyToFollowUp()
        {
            int offerKey = CreateFollowUpCase();
            //get the instance ID
            Int64 instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.FollowupHold, offerKey, true);
            //we need to update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireOnFollowupTimer, offerKey);
            //wait for timer
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.OnFollowup, instanceID, 1);
            //case should now be at the Ready to Follow Up state
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            QueryResults results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
            string prevState = results.Rows(0).Column("PreviousState").Value;
            results.Dispose();
            //ContinuewithFollowup
            _browser.ClickAction(WorkflowActivities.ApplicationManagement.ContinuewithFollowup);
            //confirm
            _browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert the case has moved back to its prev state
            X2Assertions.AssertCurrentAppManX2State(offerKey, prevState);
            //assert only one 694 and 101 OfferRole exists
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1,
                ((int)OfferRoleTypeEnum.NewBusinessProcessorD).ToString());
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void ArchiveCompletedFollowUp()
        {
            int offerKey = CreateFollowUpCase();
            //get the instance ID
            Int64 instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.FollowupHold, offerKey, true);
            //we need to update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireOnFollowupTimer, offerKey);
            //wait for timer
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.OnFollowup, instanceID, 1);
            //case should now be at the Ready to Follow Up state
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            //a scheduled activity should exist
            X2Assertions.AssertScheduleActivitySetup(offerKey.ToString(), ScheduledActivities.ApplicationManagement.ArchiveCompletedFollowup, 10, 0, 0);
            //we need to update the timer
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireArchivedCompletedFollowupTimer, offerKey);
            //wait for timer
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.ArchiveCompletedFollowup, instanceID, 1);
            X2Assertions.AssertCurrentX2State(instanceID, WorkflowStates.ApplicationManagementWF.FollowupComplete);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void UpdateFollowUp()
        {
            int offerKey = CreateFollowUpCase();
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.FollowupHold);
            _browser.ClickAction(WorkflowActivities.ApplicationManagement.UpdateFollowup);
            //update the FollowUp
            string assertHours;
            string assertMins;
            _browser.Page<MemoFollowUpAdd>().CreateFollowup(45, out assertHours, out assertMins);
            //assert that the case was sent to the correct state
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.FollowupHold, Workflows.ApplicationManagement);
            //assert that the scheduled activity was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(offerKey, ScheduledActivities.ApplicationManagement.OnFollowup, 0, Convert.ToInt32(assertHours),
                Convert.ToInt32(assertMins), 1, true);
            //assert only one 694 and 101 OfferRole exists
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            bool b = Service<IAssignmentService>().OfferRoleTypesAssignedInX2WorkFlowAssignment(offerKey, "Application Submitted", "102");
            if (b)
            {
                WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101", "102");
            }
            else WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101");
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void CompleteFollowUp()
        {
            int offerKey = CreateFollowUpCase();
            //we now need to get previous state of the application
            QueryResults instanceDetails = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
            string prevState = instanceDetails.Rows(0).Column("PreviousState").Value;
            instanceDetails.Dispose();
            _browser.Page<WorkflowSuperSearch>().Search(_browser, offerKey, WorkflowStates.ApplicationManagementWF.FollowupHold);
            //CompleteFollowup
            _browser.ClickAction(WorkflowActivities.ApplicationManagement.CompleteFollowup);
            //browser.Page<WorkflowYesNo>().Confirm(true);
            //Assert the case is now at the previous state
            X2Assertions.AssertCurrentAppManX2State(offerKey, prevState);
            //assert only one 694 and 101 OfferRole exists
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, "Followup Complete", null, 2, "694", "101");
        }

        #endregion Tests
    }
}