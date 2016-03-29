using System;
using System.Collections.Generic;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.AttorneyAccess;
using BuildingBlocks.Services.Contracts;
using Common;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.AttorneyAccess
{
    /// <summary>
    /// Contains tests for the attorney access website
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class AttorneyAccessTests : DebtCounsellingTests.AttorneyAccess.TestBase<AttorneyAccessView>
    {
        private TestBrowser browser;
        private static readonly string URL = GlobalConfiguration.AttorneyWebAccess;
        private static readonly string searchURL = GlobalConfiguration.AttorneyWebAccessSearchURL;
        private int AttorneyKey;
        private int LegalEntityKey;
        private List<int> DebtCounsellingCases;
        private string Password = "Natal1";

        #region SetupTearDown

        /// <summary>
        /// Runs when the test suite starts up
        /// </summary>
        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
            Logger.LogWriter = new ConsoleLogWriter();
            Service<IWatiNService>().KillAllIEProcesses();
            //we need an attorney
            var attorney = base.Service<IAttorneyService>().GetAttorney(true, true);
            AttorneyKey = attorney.AttorneyKey;
            LegalEntityKey = attorney.LegalEntity.LegalEntityKey;
            //we need to allocate debt counselling cases to this attorney and get the list back
            DebtCounsellingCases = Service<IDebtCounsellingService>().AssignDebtCounsellingCasesToAttorney(LegalEntityKey);
            //ensure at least one user belongs to the attorney
            var legalEntityLogin = Service<ILegalEntityService>().GetAttorneyAccessLoginLinkedToAttorney(AttorneyKey, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            if (legalEntityLogin == null)
            {
                var r = new Random();
                string emailAddress = string.Format(@"att.test{0}@test.com", r.Next(0, 100000).ToString());
                RegisterNewUser(emailAddress);
                Service<IWatiNService>().KillAllIEProcesses();
            }
            browser = new TestBrowser(string.Empty, string.Empty, URL);
            browser.BypassSSLCertificateWarning();
        }

        /// <summary>
        /// Runs on the startup of each test
        /// </summary>
        [SetUp]
        public void TestStartUp()
        {
            Settings.WaitForCompleteTimeOut = 120;
            //set passwords to Natal1
            Service<ILegalEntityService>().UpdateLegalEntityLoginPasswords();
            if (browser == null)
            {
                browser = new TestBrowser(string.Empty, string.Empty, URL);
                browser.BypassSSLCertificateWarning();
            }
        }

        /// <summary>
        /// Runs on the completion of each test
        /// </summary>
        [TearDown]
        public void TestCleanUp()
        {
            if (browser != null)
            {
                browser.Page<AttorneyAccessView>(false).LogOffLink();
            }
        }

        /// <summary>
        /// Runs on completion of the test fixture
        /// </summary>
        [TestFixtureTearDown]
        public void TestSuiteCleanUp()
        {
            Service<IWatiNService>().KillAllIEProcesses();
        }

        #endregion SetupTearDown

        /// <summary>
        /// A user with an active record in the LegalEntityLogin table can login.
        /// </summary>
        [Test, Description("A user with an active record in the LegalEntityLogin table can login.")]
        public void _01_ActiveUserCanLogin()
        {
            LoginAsActiveUser(Password);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Search");
        }

        /// <summary>
        /// If the WebAccess external role for the user is set to active, then the user can be registered.
        /// </summary>
        [Test, Description("If the WebAccess external role for the user is set to active, then the user can be registered.")]
        public void _02_ActiveWebAccessUserCanRegister()
        {
            var r = new Random();
            string emailAddress = string.Format(@"attorney.access{0}@test.co.za", r.Next(0, 100000).ToString());
            RegisterNewUser(emailAddress);
        }

        /// <summary>
        /// If the WebAccess external role for the user is set to inactive, then the user cannot be registered.
        /// </summary>
        [Test, Description("If the WebAccess external role for the user is set to inactive, then the user cannot be registered.")]
        public void _03_InactiveWebAccessUserCannotRegister()
        {
            var r = new Random();
            //we need a new LE
            string emailAddress = string.Format(@"attorney.access{0}@test.co.za", r.Next(0, 100000).ToString());
            int legalEntityKey = Service<ILegalEntityService>().CreateNewLegalEntity(emailAddress, IDNumbers.GetNextIDNumber());
            //we need to insert new external role
            Service<IExternalRoleService>().InsertExternalRole(AttorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey, legalEntityKey, ExternalRoleTypeEnum.WebAccess, GeneralStatusEnum.Inactive);
            browser.Page<AttorneyAccessView>(false).RegisterUser(emailAddress);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("This user is either deactivated or not setup for attorney web access, Contact SA Home Loans.");
        }

        /// <summary>
        /// A user that has already registered cannot register again.
        /// </summary>
        [Test, Description("A user that has already registered cannot register again.")]
        public void _04_RegisteredUserCannotRegister()
        {
            var loginUser = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            browser.Page<AttorneyAccessView>(false).RegisterUser(loginUser.Username);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The user has already been registered, please use the 'Forgot My Password' feature to recover your password.");
        }

        /// <summary>
        /// A user who has not yet registered cannot login, even if they are in the ExternalRole table as a WebAccess user.
        /// </summary>
        [Test, Description("A user who has not yet registered cannot login, even if they are in the ExternalRole table as a WebAccess user.")]
        public void _05_UnregisteredUserCannotLogin()
        {
            var r = new Random();
            //we need a new LE
            string emailAddress = string.Format(@"attorney.access{0}@test.co.za", r.Next(0, 100000).ToString());
            int legalEntityKey = Service<ILegalEntityService>().CreateNewLegalEntity(emailAddress, IDNumbers.GetNextIDNumber());
            //we need to insert new external role
            Service<IExternalRoleService>().InsertExternalRole(AttorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey, legalEntityKey, ExternalRoleTypeEnum.WebAccess, GeneralStatusEnum.Active);
            browser.Page<AttorneyAccessView>(false).LoginUser(emailAddress, Password);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(
                "The user does not exist, please use the 'Register' feature to create an account before logging in.");
        }

        /// <summary>
        /// An inactive user cannot successfully complete the Forgotten Password function, a message should be displayed to the user.
        /// </summary>
        [Test, Description("An inactive user cannot successfully complete the Forgotten Password function, a message should be displayed to the user.")]
        public void _06_InactiveUserCannotDoForgottenPassword()
        {
            var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            //set it to inactive
            string emailAddress = user.Username;
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
            browser.Page<AttorneyAccessView>(false).ForgotPassword(emailAddress);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(
                "A user with this email address has already been registered, Contact SA Home Loans.");
            //set the user back to active
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// Performing the Forgotten Password functionality will email the user a new password.
        /// </summary>
        [Test, Description("Performing the Forgotten Password functionality will email the user a new password.")]
        public void _07_ForgottenPasswordEmailsNewPassword()
        {
            var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            //perform the lost password function
            string emailAddress = user.Username;
            string date = DateTime.Now.AddMinutes(-1).ToString(Formats.DateTimeFormatSQL);
            browser.Page<AttorneyAccessView>(false).ForgotPassword(emailAddress);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(
                "Password reset was succesfull, an email containing your login details have been sent");
            ClientEmailAssertions.AssertClientEmailRecordWithSubjectAndToAddressRecordExists(emailAddress, "Attorney Web Access Registration Details",
                date);
        }

        /// <summary>
        /// This test will check that a message is displayed to the user if they are set to inactive and try to login.
        /// </summary>
        [Test, Description("This test will check that a message is displayed to the user if they are set to inactive and try to login.")]
        public void _08_InactiveUserCannotLogin()
        {
            var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            //set it to inactive
            string emailAddress = user.Username;
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
            browser.Page<AttorneyAccessView>(false).LoginUser(emailAddress, Password);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The user has been deactivated, Contact SA Home Loans.");
            //set the user back to active
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test will check that a message is displayed to the user if they are set to inactive and try to register.
        /// </summary>
        [Test, Description("This test will check that a message is displayed to the user if they are set to inactive and try to register.")]
        public void _09_InactiveUserCannotRegister()
        {
            var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            //set it to inactive
            string emailAddress = user.Username;
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
            browser.Page<AttorneyAccessView>(false).RegisterUser(emailAddress);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(
                "This user is either deactivated or not setup for attorney web access, Contact SA Home Loans.");
            Service<ILegalEntityService>().UpdateLegalEntityLogin(emailAddress, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// Searches by the whole Account Number
        /// </summary>
        [Test, Description("Searches by the whole Account Number")]
        public void _11_SearchByAccountNumber()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches by the full ID Number
        /// </summary>
        [Test, Description("Searches by the full ID Number")]
        public void _12_SearchByIDNumber()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[1];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            browser.Page<AttorneyAccessView>(false).SearchByIDNumber(idNumber);
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches by the full legal entity legal name.
        /// </summary>
        [Test, Description("Searches by the full legal entity legal name.")]
        public void _13_SearchByClientName()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[2];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //need the name
            string clientName = Service<ILegalEntityService>().GetLegalEntityLegalNameByIDNumber(idNumber);
            browser.Page<AttorneyAccessView>(false).SearchByClientName(clientName);
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches with a partial client name to ensure the search value is trimmed prior to using it to search.
        /// </summary>
        [Test, Description("Searches with a partial ID Number.")]
        public void _14_SearchByPartialIDNumber()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[1];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            browser.Page<AttorneyAccessView>(false).SearchByIDNumber(idNumber.Substring(0, 6));
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches with a partial client name.
        /// </summary>
        [Test, Description("Searches with a partial client name.")]
        public void _15_SearchByPartialClientName()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[2];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //need the name
            string clientName = Service<ILegalEntityService>().GetLegalEntityLegalNameByIDNumber(idNumber);
            browser.Page<AttorneyAccessView>(false).SearchByClientName(clientName.Substring(clientName.IndexOf(' '), 6));
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches with a partial client name with white space to ensure the search value is trimmed prior to using it to search.
        /// </summary>
        [Test, Description("Searches with a partial client name with white space to ensure the search value is trimmed prior to using it to search.")]
        public void _16_SearchByClientNameWithWhiteSpace()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[2];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //need the name
            string clientName = Service<ILegalEntityService>().GetLegalEntityLegalNameByIDNumber(idNumber);
            clientName = clientName.Substring(clientName.IndexOf(' '), 6);
            browser.Page<AttorneyAccessView>(false).SearchByClientName(string.Format(@"{0}   ", clientName));
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Searches with a partial ID number with white space to ensure the search value is trimmed prior to using it to search.
        /// </summary>
        [Test, Description("Searches with a partial ID number with white space to ensure the search value is trimmed prior to using it to search.")]
        public void _17_SearchByIDNumberWithWhiteSpace()
        {
            LoginAsUserForAttorney(AttorneyKey);
            int accountKey = DebtCounsellingCases[6];
            string idNumber = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            browser.Page<AttorneyAccessView>(false).SearchByIDNumber(string.Format(@"{0}   ", idNumber.Substring(0, 6)));
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText(string.Format(@"Account Key # {0}", accountKey));
        }

        /// <summary>
        /// Ensures that warnings are displayed when invalid data is entered into the Account Number search field.
        /// </summary>
        [Test, Description("Ensures that warnings are displayed when invalid data is entered into the Account Number search field.")]
        public void _18_SearchByInvalidAccountNumber()
        {
            var searchTerms = new List<string> { "alpha", "  1212  2", "%$%$%#$1" };
            LoginAsUserForAttorney(AttorneyKey);
            foreach (string searchTerm in searchTerms)
            {
                browser.Page<AttorneyAccessView>(false).SearchByAccountNumber(searchTerm);
                browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The field Account Number must be a number.");
            }
        }

        /// <summary>
        /// Tests that the User Name and Password fields are mandatory when trying to log in
        /// </summary>
        [Test, Description("Tests that the User Name and Password fields are mandatory when trying to log in")]
        public void _19_CheckMandatoryLoginFields()
        {
            LoginAsUser(string.Empty, string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The User name field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Password field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
            LoginAsUser(string.Empty, Password);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The User name field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
            LoginAsUser("test@test.co.za", string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Password field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
        }

        /// <summary>
        /// "Tests that the User Name is a mandatory field when trying to register"
        /// </summary>
        [Test, Description("Tests that the User Name is a mandatory field when trying to register")]
        public void _20_CheckMandatoryRegistrationFields()
        {
            browser.Page<AttorneyAccessView>(false).RegisterUser(string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The User name field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Register");
        }

        /// <summary>
        /// Tests that the User Name is a mandatory field when performing the Forgot Password feature.
        /// </summary>
        [Test, Description("Tests that the User Name is a mandatory field when performing the Forgot Password feature.")]
        public void _21_CheckMandatoryForgotPasswordFields()
        {
            browser.Page<AttorneyAccessView>(false).ForgotPassword(string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The User name field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Forgotten Password");
        }

        /// <summary>
        /// A user cannot login if their password does not match the one stored in our database.
        /// </summary>
        [Test, Description("A user cannot login if their password does not match the one stored in our database.")]
        public void _22_ActiveUserWithIncorrectPasswordCannotLogin()
        {
            LoginAsActiveUser("1237163d");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The supplied credentials are invalid, please check your username and password.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
        }

        /// <summary>
        /// Navigating directly to the URL of a page in the secure portion of the site should redirect you to the Login scren.
        /// </summary>
        [Test, Description("Navigating directly to the URL of a page in the secure portion of the site should redirect you to the Login scren.")]
        public void _23_NavigateToSecuredPageWithoutLoginIsNotAllowed()
        {
            browser.Dispose();
            try
            {
                browser = new TestBrowser(string.Empty, string.Empty, searchURL);
                browser.BypassSSLCertificateWarning();
                browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
                StringAssert.AreEqualIgnoringCase(GlobalConfiguration.AttorneyAccessCaseSearchURL.Replace(":443", string.Empty), browser.Url);
            }
            finally
            {
                browser.Dispose();
                browser = null;
            }
        }

        /// <summary>
        /// Tests that a user can log off
        /// </summary>
        [Test, Description("Tests that a user can log off")]
        public void _24_Logoff()
        {
            LoginAsActiveUser(Password);
            browser.Page<AttorneyAccessView>(false).LogOffLink();
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
            StringAssert.AreEqualIgnoringCase(GlobalConfiguration.AttorneyAccessLoginURL.Replace(":443", string.Empty), browser.Url);
        }

        /// <summary>
        /// Checks the mandatory date fields for the business event history report
        /// </summary>
        [Test, Description("Checks the mandatory date fields for the business event history report")]
        public void _25_BusinessEventHistoryReportMandatoryFields()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            browser.Page<AttorneyAccessView>(false).BusinessEventHistoryReport(string.Empty, string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Please select a From Date field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Please select a To Date field is required.");
        }

        /// <summary>
        /// Checks the mandatory date fields for the Loan Statement report
        /// </summary>
        [Test, Description("Checks the mandatory date fields for the Loan Statement report")]
        public void _26_LoanStatementReportMandatoryFields()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            browser.Page<AttorneyAccessView>(false).LoanStatementReport(string.Empty, string.Empty);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Please select a From Date field is required.");
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("The Please select a To Date field is required.");
        }

        /// <summary>
        /// The test will remove any existing proposals and then insert new ones against the debt counselling case. It will then navigate to the
        /// Proposal Details screen, ensuring that each of the proposals exist on the screen.
        /// </summary>
        [Test, Description(@"The test will remove any existing proposals and then insert new ones against the debt counselling case. It will then navigate to the
		Proposal Details screen, ensuring that each of the proposals exist on the screen.")]
        public void _27_ProposalSummaryScreenCheckData()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            //we meed proposals and counter proposals
            int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(accountKey);
            //remove any existing proposals
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Active);
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //remove any existing counter proposals
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active);
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Draft);
            //insert new ones
            Service<IProposalService>().InsertCounterProposalByStatus(debtCounsellingKey, ProposalStatusEnum.Draft, 5);
            Service<IProposalService>().InsertProposal(debtCounsellingKey, ProposalStatusEnum.Inactive, 5, TestUsers.DebtCounsellingConsultant, 1, 1);
            Service<IProposalService>().InsertProposal(debtCounsellingKey, ProposalStatusEnum.Active, 10, TestUsers.DebtCounsellingConsultant, 1, 1);
            //we need all the proposals
            List<int> proposalKeys = Service<IProposalService>().GetAllProposalsAndCounterProposalsByDebtCounsellingKey(debtCounsellingKey);
            //go to the screen
            browser.Page<AttorneyAccessView>(false).ProposalHistoryScreen();
            //ensure that each of the proposals exist
            foreach (int key in proposalKeys)
            {
                browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessProposalDetails(key);
            }
        }

        /// <summary>
        /// When a user tries to navigate to a secure portion of the login they should be redirected to the login page. Once the user logs in they
        /// should be redirected to the initial page that they attempted to access.
        /// </summary>
        [Test, Description(@"When a user tries to navigate to a secure portion of the login they should be redirected to the login page. Once the user logs in they
			should be redirected to the initial page that they attempted to access.")]
        public void _28_NavigateToSecurePageUserRedirectedAfterLogin()
        {
            browser.Dispose();
            try
            {
                browser = new TestBrowser(string.Empty, string.Empty, searchURL);
                browser.BypassSSLCertificateWarning();
                browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Log in");
                StringAssert.AreEqualIgnoringCase(GlobalConfiguration.AttorneyAccessCaseSearchURL.Replace(":443", string.Empty), browser.Url);
                var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
                browser.Page<AttorneyAccessView>(false).LoginUser(user.Username, Password);
                StringAssert.AreEqualIgnoringCase(searchURL.Replace(":443", string.Empty), browser.Url);
            }
            finally
            {
                browser.Dispose();
                browser = null;
            }
        }

        /// <summary>
        /// Runs the loan statement report and will open up the PDF document.
        /// </summary>
        [Test, Description("Runs the loan statement report and will open up the PDF document.")]
        public void _29_RunLoanStatementReport()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            string toDate = DateTime.Now.ToString(Formats.DateFormat2);
            string fromDate = DateTime.Now.AddMonths(-3).ToString(Formats.DateFormat2);
            browser.Page<AttorneyAccessView>(false).LoanStatementReport(fromDate, toDate);
        }

        /// <summary>
        /// Runs the business event history report and will open up the PDF document.
        /// </summary>
        [Test, Description("Runs the business event history report and will open up the PDF document.")]
        public void _30_RunBusinessEventHistoryReport()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            string toDate = DateTime.Now.ToString(Formats.DateFormat2);
            string fromDate = DateTime.Now.AddMonths(-3).ToString(Formats.DateFormat2);
            browser.Page<AttorneyAccessView>(false).BusinessEventHistoryReport(fromDate, toDate);
        }

        /// <summary>
        /// Runs the proposal details report and will open up the PDF document.
        /// </summary>
        [Test, Description("Runs the proposal details report and will open up the PDF document.")]
        public void _31_RunProposalDetailsReport()
        {
            this.LoginAsUserForAttorney(AttorneyKey);
            int accountKey = SearchByAccount();
            //we meed proposals and counter proposals
            int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(accountKey);
            //remove any existing proposals
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Active);
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            //remove any existing counter proposals
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Active);
            Service<IProposalService>().DeleteProposal(debtCounsellingKey, ProposalTypeEnum.Proposal, ProposalStatusEnum.Draft);
            //insert a new one
            Service<IProposalService>().InsertProposal(debtCounsellingKey, ProposalStatusEnum.Active, 10, TestUsers.DebtCounsellingConsultant, 1, 1);
            //we need all the proposals
            List<int> proposalKeys = Service<IProposalService>().GetAllProposalsAndCounterProposalsByDebtCounsellingKey(debtCounsellingKey);
            //run the report for the first proposal we find
            browser.Page<AttorneyAccessView>(false).ProposalHistoryReport(proposalKeys[0]);
        }

        #region HelperMethods

        /// <summary>
        /// Creates a new web access user and logs in as that user.
        /// </summary>
        private void LoginAsActiveUser(string password)
        {
            var user = Service<ILegalEntityService>().GetAttorneyAccessLogin(GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            browser.Page<AttorneyAccessView>(false).LoginUser(user.Username, password);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns></returns>
        private string RegisterNewUser(string emailAddress)
        {
            //we need a new LE
            int legalEntityKey = Service<ILegalEntityService>().CreateNewLegalEntity(emailAddress, IDNumbers.GetNextIDNumber());
            //we need to insert new external role
            Service<IExternalRoleService>().InsertExternalRole(AttorneyKey, GenericKeyTypeEnum.Attorney_AttorneyKey, legalEntityKey, ExternalRoleTypeEnum.WebAccess, GeneralStatusEnum.Active);
            browser = new TestBrowser(string.Empty, string.Empty, URL);
            browser.BypassSSLCertificateWarning();
            string date = DateTime.Now.AddMinutes(-1).ToString(Formats.DateTimeFormatSQL);
            browser.Page<AttorneyAccessView>(false).RegisterUser(emailAddress);
            browser.Page<BasePageAssertions>(false).AssertBrowserWindowContainsText("Registration was succesful, an email containing your registration details have been sent");
            ClientEmailAssertions.AssertClientEmailRecordWithSubjectAndToAddressRecordExists(emailAddress, "Attorney Web Access Registration Details", date);
            return emailAddress;
        }

        /// <summary>
        /// Logs in as the user provided.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name=password></param>
        private void LoginAsUser(string emailAddress, string password)
        {
            browser.Page<AttorneyAccessView>(false).LoginUser(emailAddress, password);
        }

        /// <summary>
        /// Login as an active user linked to the attorney provided.
        /// </summary>
        /// <param name="attorneyKey">AttorneyKey</param>
        private void LoginAsUserForAttorney(int attorneyKey)
        {
            var loginUser = Service<ILegalEntityService>().GetAttorneyAccessLoginLinkedToAttorney(attorneyKey, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            browser.Page<AttorneyAccessView>(false).LoginUser(loginUser.Username, Password);
        }

        /// <summary>
        /// Looks in the list of accounts that we have associated to the attorney and then loads it into the Search
        /// </summary>
        /// <returns></returns>
        private int SearchByAccount()
        {
            int accountKey = DebtCounsellingCases[0];
            browser.Page<AttorneyAccessView>(false).SearchByAccountNumber(accountKey.ToString());
            browser.Page<AttorneyAccessView>(false).AssertAttorneyAccessSearchResults(accountKey);
            browser.Page<AttorneyAccessView>(false).LoadCaseLink(accountKey);
            return accountKey;
        }

        #endregion HelperMethods
    }
}