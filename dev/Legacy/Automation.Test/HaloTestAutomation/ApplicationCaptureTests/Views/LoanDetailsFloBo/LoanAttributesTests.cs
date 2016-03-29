using System.Linq;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Collections.Generic;
using Common.Enums;

namespace ApplicationCaptureTests.Views.LoanDetailsFloBo
{
    [RequiresSTA]
    public sealed class ApplicationAttributesUpdateTests : TestBase<ApplicationAttributesUpdate>
    {
        private int _offerKey;
        private IEnumerable<Automation.DataModels.OfferAttributeType> offerAttributeTypes;

        #region Startup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //go to loan attributes
            var results = Service<IApplicationService>().GetOpenApplicationCaptureOffer();
            var row = results.Rows(0);
            offerAttributeTypes = Service<IApplicationService>().GetOfferAttributeTypes();
            this._offerKey = row.Column("offerkey").GetValueAs<int>();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(_offerKey);
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickUpdateApplicationLoanAttributesNode();
        }

        #endregion Startup/Teardown

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void when_adding_an_offer_attribute()
        {
            string attributeDescription = "HOC";
            AddAttribute(_offerKey, attributeDescription, Common.Enums.OfferAttributeTypeEnum.HOC);
        }

        [Test]
        public void when_accessing_the_offer_attributes_screen_certain_attributes_should_be_disabled()
        {
            var disabledCheckboxes = base.View.GetAttributesByEnabledState(false);
            //there should be 4
            Assert.That(disabledCheckboxes.Count() == 4, "The expected number of disabled attributes was not met.");
            //they should be QuickPay, Alpha Housing, Discounted Initiation Fee and Capitec Loan
            WatiNAssertions.AssertCheckboxExistsInCollectionByNextSiblingValue(disabledCheckboxes, "QuickPayLoan");
            WatiNAssertions.AssertCheckboxExistsInCollectionByNextSiblingValue(disabledCheckboxes, "Capitec Loan");
            WatiNAssertions.AssertCheckboxExistsInCollectionByNextSiblingValue(disabledCheckboxes, "Alpha Housing");
            WatiNAssertions.AssertCheckboxExistsInCollectionByNextSiblingValue(disabledCheckboxes, "Discounted Initiation Fee - Returning Client");
        }

        [Test]
        public void when_adding_the_old_mutual_developer_loan_attribute_the_applications_spv_is_moved_to_old_mutual()
        {
            var offer = Service<IX2WorkflowService>().GetOfferWithOfferAttributeAtState(WorkflowEnum.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                OfferAttributeTypeEnum.AlphaHousing, true);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offer.OfferKey };
            SearchForCase();
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickUpdateApplicationLoanAttributesNode();
            int existingSPVKey = Service<IApplicationService>().GetLatestOfferInformationByOfferKey(base.TestCase.OfferKey).Rows(0).Column("SPVKey").GetValueAs<int>();
            string attributeDescription = "Old Mutual Developer Loan";
            AddAttribute(base.TestCase.OfferKey, attributeDescription, Common.Enums.OfferAttributeTypeEnum.OldMutualDeveloperLoan);
            int newSPVKey = Service<IApplicationService>().GetLatestOfferInformationByOfferKey(base.TestCase.OfferKey).Rows(0).Column("SPVKey").GetValueAs<int>();
            Assert.That(newSPVKey == 160, string.Format("Expected SPVKey to be 160, it was {0}", newSPVKey));
        }

        private void AddAttribute(int offerKey, string attributeDescription, Common.Enums.OfferAttributeTypeEnum attributeEnum)
        {
            base.View.SelectAttributeByDescription(attributeDescription);
            base.View.ClickButton(Common.Enums.ButtonTypeEnum.Update);
            OfferAssertions.AssertOfferAttributeExists(offerKey, attributeEnum, true);
        }
   }
}