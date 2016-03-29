using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WorkflowAutomation.Harness;

namespace Origination.Rules
{
    [RequiresSTA]
    public class DomiciliumAddressRequired : OriginationTestBase<BasePage>
    {
        private int applicationKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);

            applicationKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType
                (
                    WorkflowStates.ApplicationManagementWF.ManageApplication,
                    Workflows.ApplicationManagement,
                    OfferTypeEnum.NewPurchase,
                    ""
                );
            Helper.CompleteEzValForOffer(base.scriptEngine, applicationKey, 1200000.00f);
        }

        protected override void OnTestStart()
        {
            Assert.AreNotEqual(0, applicationKey, "OfferKey was zero");
            Service<IApplicationService>().CleanupNewBusinessOffer(applicationKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, applicationKey.ToString()
                , new string[] { WorkflowStates.ApplicationManagementWF.ManageApplication }, ApplicationType.Any);
            base.Browser.Navigate<PropertyAddressNode>().PropertyAddress(applicationKey);
            Helper.UpdateHOCDetails(base.Browser);
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Helper.UpdateApplicationAttributes(base.Browser);
            Helper.SaveConditions(base.Browser);
            base.Browser.Page<BasePageAssertions>().AssertNoValidationMessageExists();
            Helper.CompleteDocumentChecklist(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, applicationKey.ToString()
               , new string[] { WorkflowStates.ApplicationManagementWF.ManageApplication }, ApplicationType.Any);
        }

        [Test]
        public void when_application_in_order_with_pending_domicilium_should_NOT_display_domicilium_required_error_message()
        {
            Service<IApplicationService>().UpdateAllOfferClientDomiciliumAddresses(applicationKey, GeneralStatusEnum.Pending);
            this.AssertMessageOnApplicationInOrder(false);
        }

        [Test]
        public void when_application_in_order_with_NO_domiciliums_should_display_domicilium_required_error_message()
        {
            //Clear all domicilium address
            Service<IApplicationService>().DeleteAllOfferClientDomiciliumAddresses(applicationKey);
            this.AssertMessageOnApplicationInOrder(true);
        }

        private void AssertMessageOnApplicationInOrder(bool isDisplayed)
        {
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().Confirm(true, false);
            if (isDisplayed)
                Assert.True(base.View.CheckForerrorMessages("A Domicilium Address must be captured for all Applicants on a New Business application"), "Expected a rule to fire informing user that all applicants must have a domicilium address.");
            else
                Assert.False(base.View.CheckForerrorMessages("A Domicilium Address must be captured for all Applicants on a New Business application"), "Expected a rule to NOT fire informing user that all applicants must have a domicilium address.");
        }
    }
}