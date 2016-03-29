using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
namespace FurtherLendingTests.Workflow
{
    [TestFixture, RequiresSTA]
    public class ReworkFiguresTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HelpdeskAdminUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        [Test]
        public void when_helpdesk_users_rework_further_lending_applications_the_Rework_Figures_action_should_be_applied_to_the_Awaiting_Application_state()
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.ApplicationManagement,WorkflowActivities.ApplicationManagement.ReworkFigures
                ,WorkflowStates.ApplicationManagementWF.AwaitingApplication , X2SecurityGroups.HelpDesk);
        }
        [Test]
        public void when_helpdesk_users_rework_further_lending_applications_they_should_be_able_to_update_figures_calculate_and_complete_the_Rework_Figures_activity()
        {
            int offerkey = ReworkApplicationAtAwaitingApplication();
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(offerkey, Common.Enums.StageDefinitionStageDefinitionGroupEnum.ApplicationManagement_ReworkApplication);
        }
        [Test]
        public void when_a_further_lending_application_is_reworked_the_timer_should_be_updated_to_30_days()
        {
            int offerkey = ReworkApplicationAtAwaitingApplication();
            var accountKey = Service<IApplicationService>().GetOfferAccountKey(offerkey);
            var flOfferKeys = Service<IApplicationService>().GetOpenFurtherLendingOffersAtStateByAccountKey(accountKey, WorkflowStates.ApplicationManagementWF.AwaitingApplication);
            foreach (var flOfferKey in flOfferKeys)
            {
                X2Assertions.AssertScheduledActivityTimer(flOfferKey.OfferKey.ToString(), ScheduledActivities.ApplicationManagement._30DaysTimer, 30, false);
            }
        }

        [Test]
        public void when_a_further_lending_application_is_reworked_the_application_and_confirmation_of_enquiry_are_resent()
        {
            var date = DateTime.Now;
            int offerkey = ReworkApplicationAtAwaitingApplication();
            var accountKey = Service<IApplicationService>().GetOfferAccountKey(offerkey);
            CorrespondenceAssertions.AssertCorrespondenceRecordAddedAfterDate(accountKey, CorrespondenceReports.NaedoDebitOrderAuthorization, CorrespondenceMedium.Fax, date);
            CorrespondenceAssertions.AssertCorrespondenceRecordAddedAfterDate(accountKey, CorrespondenceReports.ConfirmationofEnquiry, CorrespondenceMedium.Fax, date);

        }


        /// <summary>
        /// This test asserts that a HelpDesk user can NTU further lending application.
        /// </summary>
        [Test, Description("This test asserts that a HelpDesk user can NTU further lending application.")]
        public void when_a_helpdesk_user_wants_to_ntu_an_app_in_the_enquiry_stage_assert_that_they_can_ntu()
        {
            // Locate a case at stage 'Awaiting Application' and browse to it.
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.AwaitingApplication, Workflows.ApplicationManagement, OfferTypeEnum.FurtherLoan, "FLAutomation");
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.AwaitingApplication);
            // Assert that action 'HelpDesk NTU' is available
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsLink("HelpDesk NTU", "title");
            // Perform action 'NTU' and assert that the selected reason is written to the database.
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.HelpDeskNTU);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            ReasonAssertions.AssertReason("Client does not wish to proceed", ReasonType.ApplicationNTU, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            // Assert that after performing the 'NTU' action, the application is NTU finalised and moved to the NTU bin.
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
        }


        private int ReworkApplicationAtAwaitingApplication()
        {
            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            var offers = Service<BuildingBlocks.Services.X2WorkflowService>().GetOffersAtState(WorkflowStates.ApplicationManagementWF.AwaitingApplication, Workflows.ApplicationManagement, String.Empty);

            offers.Sort();

            int offerkey = offers[offers.Count - 1];

            base.Browser.Page<WorkflowSuperSearch>().Search(offerkey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReworkFigures);
            base.Browser.Page<FurtherLendingCalculator>()
                .SetEmploymentType()
                .ClickCalculateAndConfirmWarnings()
                .ClickNext()
                .VerifyConditions()
                .ClickNext()
                .SetLegalEntityContactOptions(CorrespondenceMedium.Fax)
                .ClickNext()
                .IncludeNaedoForms()
                .GenerateApplication();
            return offerkey;
        }
    }
}
