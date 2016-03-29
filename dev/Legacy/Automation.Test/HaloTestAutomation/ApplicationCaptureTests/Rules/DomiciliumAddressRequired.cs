using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Rules
{
    [RequiresSTA]
    public class DomiciliumAddressRequired : TestBase<BasePage>
    {
        private int applicationKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);

            applicationKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType
                (
                    WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                    Workflows.ApplicationCapture,
                    OfferTypeEnum.NewPurchase,
                    ""
                );
        }

        protected override void OnTestStart()
        {
            Service<IApplicationService>().CleanupNewBusinessOffer(applicationKey);
            base.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, applicationKey.ToString()
               , new string[] { WorkflowStates.ApplicationCaptureWF.ApplicationCapture }, ApplicationType.Any);
        }

        [Test]
        public void when_submitting_application_with_pending_domicilium_should_NOT_display_domicilium_required_error_message()
        {
            Service<IApplicationService>().UpdateAllOfferClientDomiciliumAddresses(applicationKey, GeneralStatusEnum.Pending);
            this.AssertMessageOnSubmitApplication(false);
        }

        [Test]
        public void when_submitting_application_with_NO_domiciliums_should_display_domicilium_required_error_message()
        {
            //Clear all domicilium address
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(applicationKey);
            this.AssertMessageOnSubmitApplication(true);
        }

        private void AssertMessageOnSubmitApplication(bool isDisplayed)
        {
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            if (isDisplayed)
                Assert.True(base.View.CheckForerrorMessages("A Domicilium Address must be captured for all Applicants on a New Business application"), "Expected a rule to fire informing user that all applicants must have a domicilium address.");
            else
                Assert.False(base.View.CheckForerrorMessages("A Domicilium Address must be captured for all Applicants on a New Business application"), "Expected a rule to NOT fire informing user that all applicants must have a domicilium address.");
        }
    }
}