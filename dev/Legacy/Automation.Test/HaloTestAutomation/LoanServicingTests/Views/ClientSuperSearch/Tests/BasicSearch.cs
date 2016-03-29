using Meyn.TestLink;
using NUnit.Framework;


namespace LoanServicingTests.ClientSuperSearchTests
{
    using Helpers;
    [TestFixture, RequiresSTA]
#if !DEBUG
    [TestLinkFixture(
        Url = "http://sahls31:8181/testlink/lib/api/xmlrpc.php",
        ProjectName = "HALO Automation",
        TestPlan = "Regression Tests",
        TestSuite = "Loan Servicing Tests",
        UserId = "admin",
        DevKey = "896f902c0397d7c1849290a44d0f6fb5")]
#endif
    public sealed class BasicSearch
    {
        #region TestVariables
        private string _firstnames;
        private string _surname;
        private string _idnumber;
        private string _account;
        private string _salaryNo;
        #endregion

        /// <summary>
        /// Start the browser.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
            BrowserHelpers.StartBrowser();
            BrowserHelpers.NavigateToClientSuperSearchBasic();
            TestHelpers.GetClientSearchData(out _firstnames, out _surname, out _idnumber, out _salaryNo, out _account);
        }
        
        /// <summary>
        /// Close the Browser
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BrowserHelpers.CloseBrowser();
        }

        /// <summary>
        /// Will clear the ClientSuperSearch view.
        /// </summary>
        [SetUp]
        public void TestFixture()
        {
            Helpers.TestHelpers.ClearClientSuperSearchView(true, true, true, true, true);
        }

        #region CombinationalSearchTests

        /// <summary>
        /// This test will do a firstnames and surname combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityFirstNamesSurname()
        {
            Helpers.TestHelpers.Search(_firstnames, _surname, "", "", "");
            Helpers.TestHelpers.AssertSearchResults(_firstnames, _surname,"","","");
        }

        /// <summary>
        /// This test will do a firstnames and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDFirstNames()
        {
            Helpers.TestHelpers.Search(_firstnames, "", _idnumber, "", "");
            Helpers.TestHelpers.AssertSearchResults(_firstnames, "", _idnumber, "", "");
        }

        /// <summary>
        /// This test will do a surname and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDSurname()
        {
            Helpers.TestHelpers.Search("", _surname, _idnumber, "", "");
            Helpers.TestHelpers.AssertSearchResults("", _surname, _idnumber, "", "");
        }

        /// <summary>
        /// This test will do a account and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDAccount()
        {
            Helpers.TestHelpers.Search("", "", _idnumber, "", _account);
            Helpers.TestHelpers.AssertSearchResults("", "", _idnumber, "", _account);
        }

        /// <summary>
        /// This test will do a account and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDSalary()
        {
            Helpers.TestHelpers.Search("", "", _idnumber, _salaryNo, "");
            Helpers.TestHelpers.AssertSearchResults("", "", _idnumber, _salaryNo, "");
        }

        #endregion

        #region SingleSearchTests
        /// <summary>
        /// This test will do a FirstNames search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityFirstNames()
        {
            Helpers.TestHelpers.Search(_firstnames, "", "", "", "");
            Helpers.TestHelpers.AssertSearchResults(_firstnames, "", "", "", "");
        }

        /// <summary>
        /// This test will do a Surname search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntitySurname()
        {
            Helpers.TestHelpers.Search("", _surname, "", "", "");
            Helpers.TestHelpers.AssertSearchResults("", _surname, "", "", "");
        }

        /// <summary>
        /// This test will do a Account search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityAccount()
        {
            Helpers.TestHelpers.Search("", "", "", "", _account);
            Helpers.TestHelpers.AssertSearchResults("", "", "", "", _account);
        }
        #endregion
    }
}
