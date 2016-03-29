using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;

namespace LoanServicingTests.Views.ClientSearch
{
    [TestFixture, RequiresSTA]
    public sealed class BasicSearchTests : TestBase<BasePage>
    {
        #region TestVariables

        private string _firstnames;
        private string _surname;
        private string _idnumber;
        private string _account;
        private string _salaryNo;
        private TestBrowser browser;

        #endregion TestVariables

        protected override void OnTestFixtureSetup()
        {
            Service<IWatiNService>().CloseAllOpenIEBrowsers();
            browser = new TestBrowser(TestUsers.FLSupervisor);
            Helper.NavigateToClientSuperSearchBasic(browser);
            Helper.GetClientSearchData(out _firstnames, out _surname, out _idnumber, out _salaryNo, out _account);
        }

        protected override void OnTestStart()
        {
            Helper.ClearClientSuperSearchView(browser, true, true, true, true, true);
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
            browser.Dispose();
            browser = null;
        }

        #region CombinationalSearchTests

        /// <summary>
        /// This test will do a firstnames and surname combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityFirstNamesSurname()
        {
            Helper.Search(browser, _firstnames, _surname, string.Empty, string.Empty, string.Empty);
            Helper.AssertSearchResults(browser, _firstnames, _surname, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// This test will do a firstnames and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDFirstNames()
        {
            Helper.Search(browser, _firstnames, string.Empty, _idnumber, string.Empty, string.Empty);
            Helper.AssertSearchResults(browser, _firstnames, string.Empty, _idnumber, string.Empty, string.Empty);
        }

        /// <summary>
        /// This test will do a surname and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDSurname()
        {
            Helper.Search(browser, string.Empty, _surname, _idnumber, string.Empty, string.Empty);
            Helper.AssertSearchResults(browser, string.Empty, _surname, _idnumber, string.Empty, string.Empty);
        }

        /// <summary>
        /// This test will do a account and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDAccount()
        {
            Helper.Search(browser, string.Empty, string.Empty, _idnumber, string.Empty, _account);
            Helper.AssertSearchResults(browser, string.Empty, string.Empty, _idnumber, string.Empty, _account);
        }

        /// <summary>
        /// This test will do a account and id number combination search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityIDSalary()
        {
            Helper.Search(browser, string.Empty, string.Empty, _idnumber, _salaryNo, string.Empty);
            Helper.AssertSearchResults(browser, string.Empty, string.Empty, _idnumber, _salaryNo, string.Empty);
        }

        #endregion CombinationalSearchTests

        #region SingleSearchTests

        /// <summary>
        /// This test will do a FirstNames search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityFirstNames()
        {
            Helper.Search(browser, _firstnames, string.Empty, string.Empty, string.Empty, string.Empty);
            Helper.AssertSearchResults(browser, _firstnames, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// This test will do a Surname search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntitySurname()
        {
            Helper.Search(browser, string.Empty, _surname, string.Empty, string.Empty, string.Empty);
            Helper.AssertSearchResults(browser, string.Empty, _surname, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// This test will do a Account search and then assert the results.
        /// </summary>
        [Test]
        public void LegalEntityAccount()
        {
            Helper.Search(browser, string.Empty, string.Empty, string.Empty, string.Empty, _account);
            Helper.AssertSearchResults(browser, string.Empty, string.Empty, string.Empty, string.Empty, _account);
        }

        #endregion SingleSearchTests
    }
}