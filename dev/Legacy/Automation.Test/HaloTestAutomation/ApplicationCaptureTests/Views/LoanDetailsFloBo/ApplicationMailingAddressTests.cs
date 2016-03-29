using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace ApplicationCaptureTests.Views.LoanDetailsFloBo
{
    /// <summary>
    /// Contains tests for the capturing of an application mailing address
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class ApplicationMailingAddressTests : TestBase<ApplicationMailingAddressUpdate>
    {
        #region privateVar

        /// <summary>
        /// List of offerkeys for the test
        /// </summary>
        private List<int> offerKeyList;

        #endregion privateVar

        #region setup

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            try
            {
                Service<IWatiNService>().CloseAllOpenIEBrowsers();
                base.Browser = new TestBrowser(TestUsers.BranchConsultant);
                offerKeyList = Service<IApplicationService>().GetOfferWithoutMailingAddress(4);
            }
            catch
            {
                if (base.Browser != null)
                    base.Browser.Dispose();
                base.Browser = null;
                throw;
            }
        }

        #endregion setup

        #region tests

        /// <summary>
        /// Capture an application mailing address with the Email correspondence medium option selected.
        /// </summary>
        [Test, Description("Capture an application mailing address with the Email correspondence medium option selected.")]
        public void ApplicationMailingAddressEmail()
        {
            int offerkey = offerKeyList[0];
            var results = Service<ILegalEntityService>().GetFirstLegalEntityAndFormattedAddressOnOffer(offerkey, GeneralStatusEnum.Active);
            string formattedAddress = results.Rows(0).Column("formattedaddress").Value;
            string emailaddress = results.Rows(0).Column("emailaddress").Value;
            var legalEntityKey = results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
            if (string.IsNullOrEmpty(emailaddress))
            {
                emailaddress = "clintons@sahomeloans.com";
                Service<ILegalEntityService>().UpdateEmailAddress(legalEntityKey, emailaddress);
            }
            CaptureApplicationMailingAddress(offerkey, formattedAddress, CorrespondenceMedium.Email, OnlineStatementFormat.HTML, Language.English, true, emailaddress);
        }

        /// <summary>
        /// Capture an application mailing address with the Fax correspondence medium option selected.
        /// </summary>
        [Test, Description("Capture an application mailing address with the Fax correspondence medium option selected.")]
        public void ApplicationMailingAddressFax()
        {
            int offerkey = offerKeyList[1];
            var results = Service<ILegalEntityService>().GetFirstLegalEntityAndFormattedAddressOnOffer(offerkey, GeneralStatusEnum.Active);
            string formattedAddress = results.Rows(0).Column("formattedaddress").Value;
            CaptureApplicationMailingAddress(offerKeyList[1], formattedAddress, CorrespondenceMedium.Fax, OnlineStatementFormat.NotApplicable, Language.Afrikaans, false);
        }

        /// <summary>
        /// Capture an application mailing address with the Post correspondence medium option selected.
        /// </summary>
        [Test, Description("Capture an application mailing address with the Post correspondence medium option selected.")]
        public void ApplicationMailingAddressPost()
        {
            int offerkey = offerKeyList[2];
            var results = Service<ILegalEntityService>().GetFirstLegalEntityAndFormattedAddressOnOffer(offerkey, GeneralStatusEnum.Active);
            string formattedAddress = results.Rows(0).Column("formattedaddress").Value;
            CaptureApplicationMailingAddress(offerKeyList[2], formattedAddress, CorrespondenceMedium.Post, OnlineStatementFormat.PDFFormat, Language.English, true);
        }

        /// <summary>
        /// Capture an application mailing address with the SMS correspondence medium option selected.
        /// </summary>
        [Test, Description("Capture an application mailing address with the SMS correspondence medium option selected.")]
        public void ApplicationMailingAddressSMS()
        {
            int offerkey = offerKeyList[3];
            var results = Service<ILegalEntityService>().GetFirstLegalEntityAndFormattedAddressOnOffer(offerkey, GeneralStatusEnum.Active);
            string formattedAddress = results.Rows(0).Column("formattedaddress").Value;
            CaptureApplicationMailingAddress(offerKeyList[3], formattedAddress, CorrespondenceMedium.SMS, OnlineStatementFormat.Text, Language.Afrikaans, true);
        }

        /// <summary>
        /// Helper method to capture the application mailing address
        /// </summary>
        /// <param name="offerkey">Application Number</param>
        /// <param name="formattedAddress">Formatted address string to use as the mailing address</param>
        /// <param name="correspondenceMedium">Correspondence Medium</param>
        /// <param name="onlineStatementFormat">Online Statement Format</param>
        /// <param name="language">Lanugage</param>
        /// <param name="isOnlineStatement">TRUE = select online statement option</param>
        /// <param name="emailaddress">Email address to use.</param>
        private void CaptureApplicationMailingAddress(int offerkey, string formattedAddress, string correspondenceMedium, string onlineStatementFormat,
                string language, bool isOnlineStatement, string emailaddress = null)
        {
            try
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
                base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
                base.Browser.Page<WorkflowSuperSearch>().Search(offerkey);

                base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
                base.Browser.Navigate<LoanDetailsNode>().ClickApplicationMailingAddressNode();
                base.Browser.Navigate<LoanDetailsNode>().ClickUpdateApplicationMailingAddressNode();

                if (emailaddress != null)
                {
                    base.View.UpdateApplicationMailingAddress(formattedAddress, correspondenceMedium, language, isOnlineStatement, onlineStatementFormat, emailaddress);
                }
                else
                {
                    base.View.UpdateApplicationMailingAddress(formattedAddress, correspondenceMedium, language, isOnlineStatement, onlineStatementFormat);
                }
                OfferAssertions.AssertOfferMailingAddress(offerkey, formattedAddress, correspondenceMedium, onlineStatementFormat, language, isOnlineStatement
                );
            }
            catch
            {
                if (base.Browser != null)
                    base.Browser.Dispose();
                base.Browser = null;
                throw;
            }
        }

        #endregion tests
    }
}