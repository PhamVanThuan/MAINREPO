using BuildingBlocks;
using WatiN.Core;
using WatiN.Core.Logging;
using CommonData.Constants;

namespace LoanServicingTests.ClientSuperSearchTests.Helpers
{
    public static class BrowserHelpers
    {
        private static Browser _browser;
        public static Browser GetBrowser
        {
            get
            {
                return _browser;
            }
        }
        public static void NavigateToClientSuperSearchBasic()
        {
            NavigateToClientSuperSearch();
        }
        public static void NavigateToClientSuperSearchAdvanced()
        {
            NavigateToClientSuperSearch();
        }
        private static void NavigateToClientSuperSearch()
        {
            Navigation.Base.LegalEntityMenu(_browser);
        }
        public static void StartBrowser()
        {
            Common.WatiNExtensions.CloseAllOpenIEBrowsers();
            Logger.LogWriter = new ConsoleLogWriter();
            _browser = Navigation.Base.StartBrowser(TestUsers.FLSupervisor, TestUsers.Password);
        }
        public static void CloseBrowser()
        {
            try
            {
                Navigation.Base.CheckForErrorMessages(_browser);
            }
            finally
            {
                _browser.Dispose();
                _browser = null;
            }
        }
    }
}
