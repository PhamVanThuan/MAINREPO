using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Enums;

namespace DebtCounsellingTests.Views
{
    public static class ViewsHelper
    {
        private static IDebtCounsellingService debtCounsellingService;

        static ViewsHelper()
        {
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
        }

        public static int GetDebtCounsellingCase(string statename, out string adusername)
        {
            int debtCounsellingKey = 0;
            int accountkey = 0;
            debtCounsellingService.GetDebtCounsellingCaseByState(statename, out debtCounsellingKey, out accountkey, out adusername);
            return debtCounsellingKey;
        }

        public static void NavigateSearchAndLoadDebtCounsellingCase(TestBrowser browser, int debtCounsellingKey, params string[] workflowStateName)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            var results = debtCounsellingService.GetDebtCounsellingRowByDebtCounsellingKey(debtCounsellingKey, DebtCounsellingStatusEnum.Open);
            int accountkey = results.Rows(0).Column("accountkey").GetValueAs<int>();
            WorkflowHelper.NavigateSearchAndLoadDebtCounsellingCase(browser, accountkey, workflowStateName);
        }

        public static void NavigateSearchAndLoadLoanServicingAccount(TestBrowser browser, int accountkey)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountkey);
        }
    }
}