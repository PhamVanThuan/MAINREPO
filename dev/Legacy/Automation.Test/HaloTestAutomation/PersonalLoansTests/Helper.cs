using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using System;

namespace PersonalLoansTests
{
    public static class Helper
    {
        private static IAccountService accountService;

        static Helper()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
        }

        public static void LoadLegalEntityOnCBO(TestBrowser browser, int legalentityKey, int accountkey)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Page<ClientSuperSearch>().PopulateSearch("", "", "", "", accountkey.ToString());
            browser.Page<ClientSuperSearch>().PerformSearch();
            browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(legalentityKey);
        }

        public static void LoadOfferApplicantInCBO(TestBrowser browser, long IDNumber)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Page<ClientSuperSearch>().PerformAdvancedSearch(IDNumber.ToString(), "Person", "None");
            browser.Page<ClientSuperSearch>().SelectByIDNumber(IDNumber.ToString());
        }

        internal static int FindPersonalLoanAccount(bool hasCreditLifePolicy)
        {
            var results = hasCreditLifePolicy ? accountService.GetPersonalLoanAccountWithACreditLifePolicy() : accountService.GetPersonalLoanAccountWithoutACreditLifePolicy();
            return results.Rows(0).Column("AccountKey").GetValueAs<Int32>();
        }
    }
}