using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace FurtherLendingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public class FurtherLendingPipelineTests : TestBase<CommonReasonCommonDecline>
    {
        #region tests

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.RegistrationsManager);
        }

        /// <summary>
        /// This test will ensure that when we perform the NTU action in the Origination workflow the correct
        /// Ework flag is raised and the case is moved in Ework correctly.
        /// </summary>
        [Test, Description(@"This test will ensure that when we perform the NTU action in the Origination workflow the correct
            Ework flag is raised and the case is moved in Ework correctly.")]
        public void when_a_further_lending_case_in_pipeline_is_NTUd_the_ework_case_should_move_to_NTU_Review()
        {
            //fetch a test case
            QueryResults results = Service<IEWorkService>().GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 1);
            int offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            int accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            //start the browser
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUPipeLine);
            //select the reason and return the selected reason
            string selectedReason = base.View.SelectReasonAndSubmit(ReasonType.PipelineNTU);
            //case is now at the NTU state in X2
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.NTU);
            //we need to check that the ework case has been created
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), EworkStages.NTUReview, EworkMaps.Pipeline);
            //check the reason has been added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PipelineNTU, offerKey, GenericKeyTypeEnum.Offer_OfferKey);
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            //check the offer end date
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
        }

        /// <summary>
        /// This test will ensure that if an Ework registration pipeline case is not at an Ework stage where the X2 NTU Advise action
        /// can be performed, ie Cannot NTU in Ework, then the case cannot be NTU'd in X2 via the NTU Pipeline action.
        /// </summary>
        [Test, Description(@"This test will ensure that if an Ework registration pipeline case is not at an Ework stage where the X2 NTU Advise action
        can be performed, ie Cannot NTU in Ework, then the case cannot be NTU'd in X2 via the NTU Pipeline action.")]
        public void when_the_case_is_at_a_state_where_the_X2NTUAdvise_flag_is_not_applied_a_warning_is_displayed()
        {
            //fetch a test case
            int offerKey = Service<IEWorkService>().GetPipelineCaseWhereActionNotApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 1);
            //start the browser
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUPipeLine);
            base.View.SelectReasonAndSubmit(ReasonType.PipelineNTU);
            //a validation message should be displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Unable to perform EWork Action: X2 NTU ADVISE");
        }

        /// <summary>
        /// This test will ensure that if the related Ework case has not been sent to the Resubmitted stage in the Ework workflow
        /// then the registrations user is not allowed to perform the Resubmit action in the X2 workflow.
        /// </summary>
        [Test, Description(@"This test will ensure that if the related Ework case has not been sent to the Resubmitted stage in the Ework workflow
        then the registrations user is not allowed to perform the Resubmit action in the X2 workflow.")]
        public void when_the_ework_case_is_not_at_the_resubmitted_state_the_x2_action_cannot_be_completed()
        {
            //fetch a test case
            int offerKey = Service<IEWorkService>().GetPipelineCaseInEworkStage(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, EworkStages.LodgeDocuments, 1);
            //start the browser
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Resubmit);
            base.View.SelectReasonAndSubmit(ReasonType.Resubmission);
            //a validation message should be displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Case must be at the 'Resubmitted' stage in Pipeline. It is at Lodge Documents");
        }

        /// <summary>
        /// This test will raise the external activity for an NTU from the Ework Pipeline map that will simulate an NTU occurring
        /// from the Ework Pipeline map.
        /// </summary>
        [Test, Description(@"This test will raise the external activity for an NTU from the Ework Pipeline map that will simulate an NTU occurring
        from the Ework map.")]
        public void when_the_ext_NTUPipeline_flag_is_raised_from_ework_the_x2_case_moves_to_the_NTU_state()
        {
            QueryResults results = Service<IEWorkService>().GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 1);
            int offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            int accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            //raise the flag
            Service<IX2WorkflowService>().PipeLineNTU(offerKey);
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(EworkStages.NTUReview, EworkStages.LodgeDocuments, EworkMaps.Pipeline, accountKey.ToString());
            //check that the X2 case has moved
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.NTU);
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            //check the offer end date
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
        }

        /// <summary>
        /// This test will raise the external activity for a reinstated NTU from the Ework Pipeline map that will simulate an NTU occurring
        /// from the Ework Pipeline map.
        /// </summary>
        [Test, Description(@"This test will raise the external activity for a reinstated NTU from the Ework Pipeline map that will simulate an NTU occurring from the Ework Pipeline map.")]
        public void when_the_EXTNTUReinstate_flag_is_raised_the_x2_case_moves_back_to_its_state_prior_to_the_NTU()
        {
            QueryResults results = Service<IEWorkService>().GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 1);
            int offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            int accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            string eworkStagePriorToNTU = results.Rows(0).Column("eStageName").GetValueAs<string>();
            //NTU the case
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.NTUPipeline, offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.NTU);
            //raise the flag to reinstate it
            Service<IX2WorkflowService>().PipeLineReinstateNTU(offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(eworkStagePriorToNTU, EworkStages.NTUReview, EworkMaps.Pipeline, accountKey.ToString());
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
            //offer end date has been updated to NULL
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
        }

        #endregion tests
    }
}