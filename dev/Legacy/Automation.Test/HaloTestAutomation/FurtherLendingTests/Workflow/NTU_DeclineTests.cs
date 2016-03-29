using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace FurtherLendingTests.Workflow
{
    [RequiresSTA]
    public class NTU_DeclineTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        #region Tests

        /// <summary>
        /// A FL Processor can record the client's NTU decision and move a case to the NTU state.
        /// </summary>
        [Test, Description("A FL Processor can NTU an application at the Manage Application state"), Category("Readvances")]
        public void _01_NTU()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            string assignedUser = Helper.GetFLProcessorForApplicationCheckIsActive(offerKey);
            //NTU the application
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            //assert the reason exists
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ApplicationNTU, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //assert the scheduled activity has been setup
            X2Assertions.AssertScheduledActivityTimer(offerKey.ToString(), ScheduledActivities.ApplicationManagement.NTUTimeout, 11, true);
            //case belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(assignedUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// Reinstating a Further Lending application will move the case back to the state from which is was NTU'd. This test ensures
        /// that the following occurs after the reinstate action has been completed
        /// </summary>
        [Test, Description("A FL Processor can reinstate the application after performing the NTU action."), Category("Readvances")]
        public void _02_ReinstateNTU()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            string assignedUser = Helper.GetFLProcessorForApplicationCheckIsActive(offerKey);
            //NTU the application
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //Reinstate NTU
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //case belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(assignedUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
        }

        /// <summary>
        /// Finalising an NTU by performing the NTU Finalised action will result in the workflow case being moved to the archive. A
        /// FL Processor can perform this action on a case that is at the NTU state. This test ensures that the following occurs after
        /// the NTU has been finalised
        /// </summary>
        [Test, Description(@"A FL Processor can finalise an NTU at the NTU state."), Category("Further Advances")]
        public void _03_NTUFinalised()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //NTU the application
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //Finalise the NTU
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// A FL Processor can perform a Decline on a FL case at the Manage Application state, which results in the application
        /// being moved to the Decline state in the workflow. This test ensures that the following occurs after the action has been
        /// completed
        /// </summary>
        [Test, Description("A FL Processor can Decline an application at the Manage Application state"), Category("Readvances")]
        public void _04_Decline()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.Readvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            string assignedUser = base.Browser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            //assert reason has been added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.AdministrativeDecline, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Decline);
            //assert the scheduled activity has been setup
            X2Assertions.AssertScheduleActivitySetup(offerKey.ToString(), ScheduledActivities.ApplicationManagement.DeclineTimeout, 30, 0, 0);
            //case belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(assignedUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Declined);
            results.Dispose();
        }

        /// <summary>
        /// Reinstating a Further Lending application will move the case back to the state from which is was Declined.
        /// </summary>
        [Test, Description("A FL Processor can Decline an application at the Manage Application state"), Category("Readvances")]
        public void _05_ReinstateDecline()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.Readvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            string assignedUser = Helper.GetFLProcessorForApplicationCheckIsActive(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.Decline);
            //Reinstate Decline
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateDecline);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //case belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(assignedUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
        }

        /// <summary>
        /// Finalising an NTU by performing the NTU Finalised action will result in the workflow case being moved to the archive. A
        /// FL Processor can perform this action on a case that is at the NTU state.
        /// </summary>
        [Test, Description("A FL Processor can finalise a decline at the Decline state"), Category("Readvances")]
        public void _06_DeclineFinalised()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.Readvance, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.Decline);
            //Finalise the Decline
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.DeclineFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveDecline);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Declined);
        }

        /// <summary>
        /// This test will pick up a FL case at the Manage Application state and then Decline the case. Once the required assertions have been completed then the test will update the Scheduled Activity in order to fire in a
        /// shorter period of time in order for a future test to ensure the timer is correct.
        /// </summary>
        [Test, Description(@"When a further lending case is moved to the Decline state a scheduled activity is setup to archive the
			case after a certain number of days"), Category("Further Advances")]
        public void _07_DeclineTimeoutArchivesCase()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.ManageApplication, offerKey);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            //assert reason has been added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.AdministrativeDecline, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Decline);
            //assert the scheduled activity has been setup
            X2Assertions.AssertScheduleActivitySetup(offerKey.ToString(), ScheduledActivities.ApplicationManagement.DeclineTimeout, 30, 0, 0);
            //update the scheduled activity
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireDeclineTimeoutTimer, offerKey, TestUsers.FLProcessor3);
            //assert
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveDecline);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Declined);
        }

        /// <summary>
        /// This test will pick up a FL case at the Manage Application state and then NTU the case.
        /// Once the required assertions have been completed then the test will update the Scheduled Activity in order to fire in a
        /// shorter period of time in order for a future test to ensure the timer is correct.
        /// </summary>
        [Test, Description(@"When a further lending case is moved to the NTU state a scheduled activity is setup to archive the
			case after a certain number of days"), Category("Further Advances")]
        public void _08_NTUTimeoutArchivesCase()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication,
                Workflows.ApplicationManagement, OfferTypeEnum.FurtherAdvance, "FLAutomation");
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.ManageApplication, offerKey);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            //assert the reason exists
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ApplicationNTU, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //assert the scheduled activity has been setup
            X2Assertions.AssertScheduledActivityTimer(offerKey.ToString(), ScheduledActivities.ApplicationManagement.NTUTimeout, 11, true);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireNTUTimeoutTimer, offerKey,
                TestUsers.FLProcessor);
            //assert
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// This test will reinstate by test case back to the Registration Pipeline state, ensuring that the corresponding Ework flag is correctly
        /// raised to move the case in the Ework Pipeline Map.
        /// </summary>
        [Test, Description(@"This test will reinstate the test case back to the Registration Pipeline state, ensuring that the corresponding Ework flag is correctly
                raised to move the case in the Ework Pipeline Map.")]
        public void _09_ReinstatePipelineNTU()
        {
            QueryResults results = Service<IEWorkService>().GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 1);
            int offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            int accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            string eworkStagePriorToNTU = results.Rows(0).Column("eStageName").GetValueAs<string>();
            //NTU the case
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.NTUPipeline, offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.NTU);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case has been reinstated in X2
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), eworkStagePriorToNTU, EworkMaps.Pipeline);
            //offer status has been updated to OPEN
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
            //offer end date has been updated to NULL
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
        }

        #endregion Tests
    }
}