using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using System;
using System.Collections.Generic;
using WatiN.Core;

namespace LoanServicingTests
{
    public static class Helper
    {
        private static ILegalEntityService legalEntityService;

        static Helper()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        public static TestBrowser LoginAndSearch(string testUser, string password, int accountKey)
        {
            // open browser with test user
            TestBrowser browser = new TestBrowser(testUser, password);
            // search for the account
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //remove any nodes from CBO
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            return browser;
        }

        public static void NavigateToClientSuperSearchBasic(TestBrowser browser)
        {
            NavigateToClientSuperSearch(browser);
        }

        public static void NavigateToClientSuperSearchAdvanced(TestBrowser browser)
        {
            NavigateToClientSuperSearch(browser);
        }

        private static void NavigateToClientSuperSearch(TestBrowser browser)
        {
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
        }

        public static void Search(TestBrowser browser, string legalEntityFirstNames, string legalentitySurname, string idNumber, string salaryNo, string accountkey)
        {
            if (!legalEntityFirstNames.Equals(String.Empty))
                browser.Page<ClientSuperSearch>().PopulateSearch(legalEntityFirstNames, "", "", "", "");
            if (!legalentitySurname.Equals(String.Empty))
                browser.Page<ClientSuperSearch>().PopulateSearch("", legalentitySurname, "", "", "");
            if (!idNumber.Equals(String.Empty))
                browser.Page<ClientSuperSearch>().PopulateSearch("", "", idNumber, "", "");
            if (!salaryNo.Equals(String.Empty))
                browser.Page<ClientSuperSearch>().PopulateSearch("", "", "", salaryNo, "");
            if (!accountkey.Equals(String.Empty))
                browser.Page<ClientSuperSearch>().PopulateSearch("", "", "", "", accountkey);
            browser.Page<ClientSuperSearch>().PerformSearch();
        }

        public static void AssertSearchResults(TestBrowser browser, string legalEntityFirstNames, string legalentitySurname, string idNumber, string salaryNo,
                    string accountkey)
        {
            List<string> expressions = new List<string>();
            bool validRows = true;

            if (!legalEntityFirstNames.Equals(String.Empty))
                expressions.Add(legalEntityFirstNames.ToLower());
            if (!legalentitySurname.Equals(String.Empty))
                expressions.Add(legalentitySurname.ToLower());
            if (!idNumber.Equals(String.Empty))
                expressions.Add(idNumber.ToLower());
            if (!salaryNo.Equals(String.Empty))
                expressions.Add(salaryNo.ToLower());
            if (!accountkey.Equals(String.Empty))
                expressions.Add(accountkey.ToLower());

            Table t = browser.Page<ClientSuperSearch>().GetResultsGrid();
            for (int index = 1; index < t.TableRows.Count; index++)
            {
                foreach (Image img in t.TableRows[index].Images)
                {
                    img.Click();
                }
                string text = t.TableRows[index].Text.ToLower();

                foreach (string expStr in expressions)
                {
                    if (text.Contains(expStr.ToLower()))
                        validRows = true;
                }
                if (!validRows)
                    NUnit.Framework.Assert.True(validRows, "The ClientSuperSearch result set contains clients that doesn't match the firstnames and surname");
            }
        }

        public static void ClearClientSuperSearchView(TestBrowser browser, bool legalEntityFirstNames, bool legalentitySurname, bool idNumber, bool salaryNo,
                bool accountkey)
        {
            if (legalEntityFirstNames)
                browser.Page<ClientSuperSearch>().Clear(true, false, false, false, false);
            if (legalentitySurname)
                browser.Page<ClientSuperSearch>().Clear(false, true, false, false, false);
            if (idNumber)
                browser.Page<ClientSuperSearch>().Clear(true, false, true, false, false);
            if (salaryNo)
                browser.Page<ClientSuperSearch>().Clear(true, false, false, true, false);
            if (accountkey)
                browser.Page<ClientSuperSearch>().Clear(true, false, false, false, true);
        }

        public static void GetClientSearchData(out string legalEntityFirstNames, out string legalentitySurname, out string idNumber, out string salaryNo,
                out string accountkey)
        {
            QueryResults results = legalEntityService.ClientSuperSearchFirstLegalEntity();
            legalEntityFirstNames = results.Rows(0).Column("firstnames").Value;
            legalentitySurname = results.Rows(0).Column("surname").Value;
            idNumber = results.Rows(0).Column("idnumber").Value;
            salaryNo = results.Rows(0).Column("salarynumber").Value;
            accountkey = results.Rows(0).Column("accountkey").Value;
            results.Dispose();
        }
    }
}