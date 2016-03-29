using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class FollowUpTests : ApplicationCaptureTests.TestBase<MemoFollowUpAdd>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        [Test, Sequential, Description("Data Setup for tests required on cases at the Ready to Follow Up state")]
        public void _01_ReadyToFollowUp([Values("ReinstateFollowUp", "ContinueWithApplicationReadyToFollowup")] string identifier)
        {
            base.GetTestCase(identifier);
            //begin the test
            base.SearchForCase();
            //Create the Follow Up
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateFollowup);
            string selectedHour; string selectedMinute;
            base.View.CreateFollowup(5, out selectedHour, out selectedMinute);
            //assert that the case was sent to the correct state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey, ScheduledActivities.ApplicationCapture.WaitforFollowup, selectedHour, selectedMinute);
            //fire the timer for the following test cases to use
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.FireWaitForFollowupTimer, base.TestCase.OfferKey);
            //should be at ready to follow up
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ReadytoFollowup);
        }

        /// <summary>
        /// Verify that a Branch Consultant can perform the Create Followup action
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can perform the Create Followup action")]
        public void _02_CreateFollowUp()
        {
            base.GetTestCase("CreateFollowUp");
            //login as Branch Consultant
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateFollowup);
            //create the FollowUp
            string hourValueToAssert; string minValueToAssert;
            base.View.CreateFollowup(15, out hourValueToAssert, out minValueToAssert);
            //assert that the case was sent to the correct state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey, ScheduledActivities.ApplicationCapture.WaitforFollowup, hourValueToAssert, minValueToAssert);
            CheckWorkflowAssignment();
        }

        /// <summary>
        /// Verify that a Branch Consultant can create a follow up and continue with the application
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can create a follow up and continue with the application")]
        public void _03_ContinueApplicationFromFollowUpHold()
        {
            base.GetTestCase("ContinueApplicationFromFollowUpHold");
            //login as Branch Consultant
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateFollowup);
            //create the FollowUp
            string hourValueToAssert; string minValueToAssert;
            base.View.CreateFollowup(10, out hourValueToAssert, out minValueToAssert);
            //assert that the case was sent to the correct state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.FollowupHold);
            //we now need to get previous state of the application
            var results = Service<IX2WorkflowService>().GetAppCapInstanceDetails(base.TestCase.OfferKey);
            string prevState = results.Rows(0).Column("Last_State").Value;
            //Continue Application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContinueApplication);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assert the case is now at the previous state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, prevState);
            CheckWorkflowAssignment();
        }

        /// <summary>
        /// Verify that a branch consultant user can Reinstate a Follow Up from the Ready to Follow Up state
        /// </summary>
        [Test, Description("Verify that a branch consultant user can Reinstate a Follow Up from the Ready to Follow Up state")]
        public void _04_ReinstateFollowUp()
        {
            base.GetTestCase("ReinstateFollowUp");
            //login as Branch Consultant
            base.SearchForCase();
            //reinstate
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReinstateFollowup);
            //create a new follow up time
            string hourToAssert; string minuteToAssert;
            base.View.CreateFollowup(5, out hourToAssert, out minuteToAssert);
            //assert that the case is back at Followup Hold
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey, ScheduledActivities.ApplicationCapture.WaitforFollowup, hourToAssert, minuteToAssert);
            CheckWorkflowAssignment();
        }

        /// <summary>
        /// Verify that a branch consultant user can continue with the Application from the Ready to Follow Up state
        /// </summary>
        [Test, Description("Verify that a branch consultant user can continue with the Application from the Ready to Follow Up state")]
        public void _05_ContinueWithApplicationReadyToFollowup()
        {
            base.GetTestCase("ContinueWithApplicationReadyToFollowup");
            //fetch the previous state
            var results = Service<IX2WorkflowService>().GetAppCapInstanceDetails(base.TestCase.OfferKey);
            string prevState = results.Rows(0).Column("Last_State").Value;
            base.SearchForCase();
            //continue with the application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContinuewithApplication);
            //confirm
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert the case has moved back to its prev state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, prevState);
            CheckWorkflowAssignment();
        }

        /// <summary>
        /// Verify that a Branch Consultant can perform the Update Followup action
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can perform the Update Followup action")]
        public void _06_UpdateFollowUp()
        {
            base.GetTestCase("UpdateFollowUp");
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateFollowup);
            //create the FollowUp
            string hourValue; string minValue;
            base.View.CreateFollowup(10, out hourValue, out minValue);
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateFollowup);
            //update the FollowUp
            string assertHours; string assertMins;
            base.View.CreateFollowup(45, out assertHours, out assertMins);
            //assert that the case was sent to the correct state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey, ScheduledActivities.ApplicationCapture.WaitforFollowup, assertHours, assertMins);
            CheckWorkflowAssignment();
        }

        private void CheckWorkflowAssignment()
        {
            //we need to check the 101 OfferRole
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
            //we need to check the 101 WFA record
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD, base.TestCase.Username);
        }
    }
}