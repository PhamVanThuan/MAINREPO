using WatiN.Core;
using BuildingBlocks;

namespace DebtCounselling.Workflow
{
    public static class Helper
    {
        public static Browser StartBrowserAsDebtCounsellingAdmin()
        {
            return BuildingBlocks.Navigation.Base.StartBrowser
                (
                    DebtCounsellingSettings.Default.TestUserPassword,
                    DebtCounsellingSettings.Default.DebtCounsellingAdminUser
                );
        }
        public static Browser StartBrowserAsDebtCounsellingConsultant()
        {
            return BuildingBlocks.Navigation.Base.StartBrowser
           (
               DebtCounsellingSettings.Default.TestUserPassword,
               DebtCounsellingSettings.Default.DebtCounsellingConsultantUser
           );
        }
        public static Browser StartBrowserAsCounsellingSupervisor()
        {
            return BuildingBlocks.Navigation.Base.StartBrowser
            (
                DebtCounsellingSettings.Default.TestUserPassword,
                DebtCounsellingSettings.Default.DebtCounsellingSupervisorUser
            );
        }
        public static Browser StartBrowserAsDebtCounsellingCourtConsultant()
        {
            return BuildingBlocks.Navigation.Base.StartBrowser
            (
                DebtCounsellingSettings.Default.TestUserPassword,
                DebtCounsellingSettings.Default.DebtCounsellingCourtConsultantUser
            );
        }
        public static Browser StartBrowserAsDebtCounsellingManager()
        {
            return BuildingBlocks.Navigation.Base.StartBrowser
            (
                DebtCounsellingSettings.Default.TestUserPassword,
                DebtCounsellingSettings.Default.DebtCounsellingManagerUser
            );
        }
        public static void NavigateAndSelectDebtCounsellor(Browser b, string debtCounsellorName)
        {
            Views.DebtCounsellorSelect.SelectTier(b, debtCounsellorName);
            Views.DebtCounsellorSelect.ClickButton(b, Views.DebtCounsellorSelect.btn.Select);
        }
        public static void AddDebtCounsellingInitiatorReasons(Browser b,params  string [] initiatorReasons)
        {

        }
    }
}
