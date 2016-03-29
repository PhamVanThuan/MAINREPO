using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Threading;

namespace FurtherLendingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public class FollowUps : TestBase<BasePage>
    {
        private int _followUpOfferKey;
        private int _reinstateTimerKey;
        private int _continueTimerKey;

        #region setupTearDown

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        #endregion setupTearDown

        #region Tests

        /// <summary>
        /// This test will ensure that a further lending processor user can create a follow up cloned workflow case on a further lending
        /// application in the Readvance Payments workflow. The case is created in the Followup Hold workflow state.
        /// </summary>
        [Test, Description(@"Creates a Follow Up on a further lending application."), Category("FollowUp")]
        public void _01_CreateFollowUp()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            _followUpOfferKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule, Workflows.ReadvancePayments,
                                                              OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule);
            //we need the current user
            string flAppProcUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(_followUpOfferKey, OfferRoleTypeEnum.FLProcessorD);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.CreateFollowup);
            //create the follow up, we do not want this case to expire so its set to 20 minutes.
            string hour; string minute;
            base.Browser.Page<MemoFollowUpAdd>().CreateFollowup(5, out hour, out minute);
            //assert the creation
            Thread.Sleep(2000);
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.ReadvancePayments, _followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, flAppProcUser);
            X2Assertions.AssertScheduleActivitySetup(_followUpOfferKey, ScheduledActivities.ReadvancePayments.OnFollowup, 0, Convert.ToInt32(hour), Convert.ToInt32(minute), 1, true);
        }

        /// <summary>
        /// In order to reinstate a follow up we require a case at the Ready to Followup state. This test updates the schedule activity
        /// for OnFollowUp to ensure that the later test case will have an application to work with.
        /// </summary>
        [Test, Description(@"Sets up a test case for the reinstate follow up test case"), Category("FollowUp")]
        public void _02_SetUpReinstateFollowUp()
        {
            _reinstateTimerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule, Workflows.ReadvancePayments,
                                                              OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _reinstateTimerKey, WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule);
            //we need the current user
            string flAppProcUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(_reinstateTimerKey, OfferRoleTypeEnum.FLProcessorD);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.CreateFollowup);
            //create the follow up
            string hour; string minute;
            base.Browser.Page<MemoFollowUpAdd>().CreateFollowup(5, out hour, out minute);
            Thread.Sleep(2000);
            //assert the creation
            int instanceID = X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.ReadvancePayments, _reinstateTimerKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, flAppProcUser);
            X2Assertions.AssertScheduleActivitySetup(_reinstateTimerKey, ScheduledActivities.ReadvancePayments.OnFollowup, 0, Convert.ToInt32(hour), Convert.ToInt32(minute), 1, true);
            //update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.OnFollowupTimer, _reinstateTimerKey);
        }

        /// <summary>
        /// In order to continue a follow up we require a case at the Ready to Followup state. This test updates the schedule activity
        /// for OnFollowUp to ensure that the later test case will have an application to work with.
        /// </summary>
        [Test, Description(@"Sets up a test case for the continue follow up test case"), Category("FollowUp")]
        public void _03_SetUpContinueWithFollowUp()
        {
            _continueTimerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule, Workflows.ReadvancePayments,
                                                              OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _continueTimerKey, WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule);
            //we need the current user
            string flAppProcUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(_continueTimerKey, OfferRoleTypeEnum.FLProcessorD);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.CreateFollowup);
            //create the follow up
            string hour; string minute;
            base.Browser.Page<MemoFollowUpAdd>().CreateFollowup(5, out hour, out minute);
            Thread.Sleep(2000);
            //assert the creation
            int instanceID = X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.ReadvancePayments, _continueTimerKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, flAppProcUser);
            X2Assertions.AssertScheduleActivitySetup(_continueTimerKey, ScheduledActivities.ReadvancePayments.OnFollowup, 0, Convert.ToInt32(hour), Convert.ToInt32(minute), 1, true);
            scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.OnFollowupTimer, _continueTimerKey);
        }

        /// <summary>
        /// Once the cloned case has been created in the Followup Hold state the user can update the Follow Up time using the
        /// Update Followup action. This will update the follow up time that is stored on the associated memo record.
        /// </summary>
        [Test, Description(@"Updates a Follow Up on a further lending application."), Category("FollowUp")]
        public void _04_UpdateFollowUp()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold);
            string flAppProcUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(_followUpOfferKey, OfferRoleTypeEnum.FLProcessorD);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.UpdateFollowup);
            //update the follow up
            string minute; string hour;
            base.Browser.Page<MemoFollowUpAdd>().CreateFollowup(5, out hour, out minute);
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.ReadvancePayments, _followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, flAppProcUser);
            X2Assertions.AssertScheduleActivitySetup(_followUpOfferKey, ScheduledActivities.ReadvancePayments.OnFollowup, 0, Convert.ToInt32(hour), Convert.ToInt32(minute), 1, true);
        }

        /// <summary>
        /// A further lending user is allowed to complete the Follow Up before the timer has elapsed by performing the Complete Followup
        /// action on the workflow case. This will send the cloned case to the Followup Complete archive.
        /// </summary>
        [Test, Description(@"Completes a Follow Up on a further lending application."), Category("FollowUp")]
        public void _05_CompleteFollowUp()
        {
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.CompleteFollowup);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //ensure the case has been archived
            X2Assertions.AssertX2CloneDoesNotExist(_followUpOfferKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, Workflows.ReadvancePayments);
        }

        /// <summary>
        /// Once the follow up time has been reached the case will be moved to the Ready to Followup state. At this state the user has the
        /// ability to Reinstate the Follow Up by updating the follow up time for the application.
        /// </summary>
        [Test, Description(@"Reinstates a Follow Up on a further lending application."), Category("FollowUp")]
        public void _06_ReinstateFollowUp()
        {
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _reinstateTimerKey, WorkflowStates.ReadvancePaymentsWF.ReadyToFollowup);
            string flAppProcUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(_reinstateTimerKey, OfferRoleTypeEnum.FLProcessorD);
            //reinstate the follow up
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.ReinstateFollowup);
            string minute; string hour;
            base.Browser.Page<MemoFollowUpAdd>().CreateFollowup(5, out hour, out minute);
            Thread.Sleep(2000);
            X2Assertions.AssertWorkflowInstanceExistsForStateAndADUser(Workflows.ReadvancePayments, _reinstateTimerKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, flAppProcUser);
            X2Assertions.AssertScheduleActivitySetup(_reinstateTimerKey, ScheduledActivities.ReadvancePayments.OnFollowup, 0, Convert.ToInt32(hour), Convert.ToInt32(minute), 1, true);
        }

        /// <summary>
        /// When the follow up time has expired the user can close the follow up by performing the Continue with Follow
        /// up action. This will send the cloned case to the Followup Complete archive.
        /// </summary>
        [Test, Description(@"Continues with the application after a Follow Up has been created."), Category("FollowUp")]
        public void _07_ContinueWithFollowUp()
        {
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _continueTimerKey, WorkflowStates.ReadvancePaymentsWF.ReadyToFollowup);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.ContinuewithFollowup);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //ensure the case has been archived
            X2Assertions.AssertX2CloneDoesNotExist(_continueTimerKey, WorkflowStates.ReadvancePaymentsWF.FollowupHold, Workflows.ReadvancePayments);
        }

        #endregion Tests
    }
}