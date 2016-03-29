using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.InternetComponents.ClientSecureWebsite;
using BuildingBlocks.Services.Contracts;
using Common;
using Common.Constants;
using NUnit.Framework;
using System;

namespace InternetComponentTests.ClientSecureWebsite
{
    [RequiresSTA]
    public sealed class ClientSecureWebsiteTests : TestBase<ClientSecureWebsiteLogin>
    {
        private const string Password = "Natal1";

        [TestFixtureSetUp]
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(string.Empty, string.Empty, GlobalConfiguration.ClientSecureWebsiteURL.ToString());
            base.Browser.BypassSSLCertificateWarning();
        }

        [TearDown]
        protected override void OnTestTearDown()
        {
            if (this.UserIsLoggedIn)
            {
                this.UserIsLoggedIn = base.View.LogOff();
            }
        }

        [Test]
        public void when_a_legalentity_has_an_old_password_they_should_be_reregistered_and_their_old_login_details_removed()
        {
            LegalEntity legalEntity = base.Service<ILegalEntityService>().GetClientWithExistingPassword();
            UserIsLoggedIn = base.View.Login(legalEntity.EmailAddress, legalEntity.Password);
            LegalEntityAssertions.AssertLegalEntityRegisteredForSecureWebsite(legalEntity.LegalEntityKey);
            legalEntity = base.Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.Password == string.Empty);
        }

        [Test]
        public void when_a_legalentity_has_been_registered_they_should_be_able_to_login()
        {
            base.Service<ILegalEntityService>().UpdateLegalEntityLoginPasswords();
            LegalEntity legalEntity = base.Service<ILegalEntityService>().GetClientWithAccessToSecureWebsite();
            base.View.Login(legalEntity.EmailAddress, Password);
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.ClientSecureWebsiteLoanStatementURL.ToString(), base.Browser.Url);
        }

        [Test]
        public void when_registering_a_legalentity_login_details_are_sent_and_legalentity_is_registered()
        {
            LegalEntity legalEntity = base.Service<ILegalEntityService>().GetClientWhoHasNeverRegisteredForSecureWebsite();
            string date = DateTime.Now.AddMinutes(-1).ToString(Formats.DateTimeFormatSQL);
            base.View.ResetPassword(legalEntity.EmailAddress);
            LegalEntityAssertions.AssertLegalEntityRegisteredForSecureWebsite(legalEntity.LegalEntityKey);
            ClientEmailAssertions.AssertClientEmailRecordWithSubjectAndToAddressRecordExists(legalEntity.EmailAddress, "Web Access Authentication Details: SA Home Loans", date);
        }

        [Test]
        public void when_logging_in_a_valid_email_address_is_required()
        {
            base.View.ClickLogin();
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The E-mail address field is required.");
            base.View.Login("clint@gmail", string.Empty);
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Please enter a valid Email Address");
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.ClientSecureWebsiteURL.ToString(), base.Browser.Url);
        }

        [Test]
        public void when_logging_in_a_password_is_required()
        {
            base.View.ClickLogin();
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Password field is required.");
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.ClientSecureWebsiteURL.ToString(), base.Browser.Url);
        }

        [Test]
        public void when_resetting_password_a_valid_email_address_is_required()
        {
            base.View.ResetPassword(string.Empty);
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The E-mail address field is required.");
            base.View.ClickCancel();
            base.View.ResetPassword("bob@.");
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Please enter a valid Email Address");
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.ClientSecureWebsiteResetPasswordURL.ToString(), base.Browser.Url);
            base.View.ClickCancel();
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.ClientSecureWebsiteURL.ToString(), base.Browser.Url);
        }

        [Test]
        public void when_resetting_password_for_an_email_address_that_doesnt_exist_no_login_details_are_sent()
        {
            string emailAddress = "thisEmailAddressShouldNeverExist@gmail.com";
            string date = DateTime.Now.AddMinutes(-1).ToString(Formats.DateTimeFormatSQL);
            base.View.ResetPassword(emailAddress);
            ClientEmailAssertions.AssertNoClientEmailRecordExists(emailAddress, "Web Access Authentication Details: SA Home Loans", date);
            base.Browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(
                @"You should receive details to assist you shortly. If you have not received an e-mail in 20 minutes, please contact the helpdesk (details listed at the bottom of this page) to confirm your e-mail address");
        }

        [Test]
        public void when_an_existing_client_resets_their_password_an_email_is_sent_containing_their_new_password()
        {
            base.Service<ILegalEntityService>().UpdateLegalEntityLoginPasswords();
            Automation.DataModels.LegalEntity legalEntity = base.Service<ILegalEntityService>().GetClientWithAccessToSecureWebsite();
            base.View.ResetPassword(legalEntity.EmailAddress);
            LegalEntityAssertions.AssertLegalEntityLoginHasChanged(legalEntity.LegalEntityKey, Common.Constants.Passwords.HashedVersionofNatal1);
        }

        public bool UserIsLoggedIn { get; set; }
    }
}