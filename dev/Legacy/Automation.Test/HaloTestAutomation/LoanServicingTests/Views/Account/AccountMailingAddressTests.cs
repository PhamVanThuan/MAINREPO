using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.AccountMailingAddress;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class AccountMailingAddressTests : TestBase<AccountMailingAddressUpdate>
    {
        private IEnumerable<Automation.DataModels.AccountKeyWithIndicator> accounts;
        private Automation.DataModels.AccountKeyWithIndicator account;

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            // open browser with test user
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            // get test accounts
            accounts = Service<IAccountService>().GetAccountMailingAddressInfo();
            // load test account
            LoadAccount(string.Empty);
        }

        protected override void OnTestTearDown()
        {
            if (new IECollection().Count > 0)
            {
                base.OnTestTearDown();
            }
            else
            {
                base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
                LoadAccount(string.Empty);
            }
        }

        #endregion SetupTearDown

        #region View Tests

        /// <summary>
        /// Check that the AccountMailingAddressUpdate controls exists
        /// </summary>
        [Test]
        public void CheckAccountMailingAddressUpdateControlsExist()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.AssertAccountMailingAddressUpdateControlsExist();
        }

        /// <summary>
        /// Check that the AccountMailingAddressUpdate controls are enabled
        /// </summary>
        [Test]
        public void CheckAccountMailingAddressUpdateControlsEnabled()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.AssertAccountMailingAddressUpdateControlsEnabled();
        }

        /// <summary>
        /// Check the dropdown options in the CorrespondenceMedium dropdown list
        /// </summary>
        [Test]
        public void CheckCorrespondenceMediumOptions()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            List<string> correspondenceMedium = new List<string>() {
                        CorrespondenceMedium.Post,
                        CorrespondenceMedium.Email,
                        CorrespondenceMedium.Fax,
                        CorrespondenceMedium.SMS
                    };
            base.View.AssertCorrespondenceMediumOptions(correspondenceMedium);
        }

        /// <summary>
        /// Check the dropdown options in the CorrespondenceLanguage dropdown list
        /// </summary>
        [Test]
        public void CheckCorrespondenceLanguageOptions()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            List<string> correspondenceLanguage = new List<string>() {
                        Language.English,
                        Language.Afrikaans
                    };
            base.View.AssertCorrespondenceLanguageOptions(correspondenceLanguage);
        }

        /// <summary>
        /// Check the dropdown options in the OnlineStatementFormat dropdown list
        /// </summary>
        [Test]
        public void CheckOnlineStatementFormatOptions()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            List<string> onlineStatmentFormat = new List<string>() {
                        OnlineStatementFormat.HTML,
                        OnlineStatementFormat.NotApplicable,
                        OnlineStatementFormat.PDFFormat,
                        OnlineStatementFormat.Text,
                    };
            base.View.AssertOnlineStatmentFormatOptions(onlineStatmentFormat);
        }

        /// <summary>
        /// Check that the OnlineStatementFormat dropdown is enabled when the OnlineStatement checkbox is checked
        /// </summary>
        [Test]
        public void CheckOnlineStatementFormatEnabled_OnlineStatementChecked()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SetOnlineStatmentCheckbox(true);
            base.View.AssertOnlineStatementFormatEnabled(true);
        }

        /// <summary>
        /// Check that the OnlineStatementFormat dropdown is disabled when the OnlineStatement checkbox is unchecked
        /// </summary>
        [Test]
        public void CheckOnlineStatementFormatDisabled_OnlineStatementUnchecked()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SetOnlineStatmentCheckbox(false);
            base.View.AssertOnlineStatementFormatEnabled(false);
        }

        /// <summary>
        /// Check that the NotApplicable option is set in the OnlineStatementFormat dropdown the OnlineStatement checkbox is unchecked
        /// </summary>
        [Test]
        public void CheckOnlineStatementFormatSelection_OnlineStatementUnchecked()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SetOnlineStatmentCheckbox(false);
            base.View.AssertOnlineStatementFormatEnabled(false);
            base.View.AssertOnlineStatementFormatSelection(OnlineStatementFormat.NotApplicable);
        }

        /// <summary>
        /// Assert that it is possible to update the CorrespondenceMedium to Post
        /// </summary>
        [Test]
        public void UpdateCorrespondenceMedium_Post()
        {
            LoadAccount(CorrespondenceMedium.Post);
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SelectNewCorrespondenceMedium(CorrespondenceMedium.Post);
            var updatedValue = base.View.SelectNewMailingAddress();
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertCorrespondenceMediumValue(CorrespondenceMedium.Post);
            base.Browser.Page<AccountMailingAddress>().AssertMailingAddressValue(updatedValue);
        }

        /// <summary>
        /// Assert that it is possible to update the CorrespondenceMedium to Email
        /// </summary>
        [Test]
        public void UpdateCorrespondenceMedium_Email()
        {
            LoadAccount(CorrespondenceMedium.Email);
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SelectNewCorrespondenceMedium(CorrespondenceMedium.Email);
            var updatedValue = base.View.SelectNewEmailAddress();
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertCorrespondenceMediumValue(CorrespondenceMedium.Email);
            base.Browser.Page<AccountMailingAddress>().AssertEmailAddressValue(updatedValue);
        }

        /// <summary>
        /// Assert that it is possible to update the CorrespondenceMedium to Fax and Email
        /// </summary>
        [Test]
        [Sequential]
        public void UpdateCorrespondenceMedium_Other([Values(CorrespondenceMedium.Fax, CorrespondenceMedium.SMS)] string correspondenceMedium)
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SelectNewCorrespondenceMedium(correspondenceMedium);
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertCorrespondenceMediumValue(correspondenceMedium);
        }

        /// <summary>
        /// Assert that it is possible to update the CorrespondenceLanguage
        /// </summary>
        [Test]
        public void UpdateCorrespondenceLanguage()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            var updatedValue = base.View.SelectNewCorrespondenceLanguage();
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertCorrespondenceLanguageValue(updatedValue);
        }

        /// <summary>
        /// Assert that it is possible to udpate the OnlineStatement status
        /// </summary>
        [Test]
        public void UpdateOnlinestatement()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            var updatedValue = base.View.SetNewOnlineStatementStatus();
            if (updatedValue)
                base.View.SelectNewOnlineStatmentFormat();
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertOnlineStatementstatus(updatedValue);
        }

        /// <summary>
        /// Assert that it is possible to update the OnlineStatmentFormat
        /// </summary>
        [Test]
        public void UpdateOnlineStatmentFormat()
        {
            base.Browser.Navigate<LoanServicingCBO>().AccountMailingAddress(NodeTypeEnum.Update);
            base.View.SetOnlineStatmentCheckbox(true);
            var updatedValue = base.View.SelectNewOnlineStatmentFormat();
            base.View.ClickButton(ButtonTypeEnum.Submit);
            base.Browser.Page<AccountMailingAddress>().AssertOnlineStatmentFormatValue(updatedValue);
        }

        #endregion View Tests

        #region Helpers

        public void LoadAccount(string test)
        {
            // remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            // select account from test accounts
            account = (from a in accounts
                       where a.Count > 1 && (a.Indicator == test || string.IsNullOrEmpty(test))
                       select a).FirstOrDefault();
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(account.AccountKey);
        }

        #endregion Helpers
    }
}