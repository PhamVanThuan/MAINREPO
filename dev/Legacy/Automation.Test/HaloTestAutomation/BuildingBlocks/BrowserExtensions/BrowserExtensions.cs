using System.Threading;
using WatiN.Core;

namespace BuildingBlocks
{
    public static class BrowserExtensions
    {
        public static void WaitForAjaxRequest(this TestBrowser TestBrowser)
        {
            int timeWaitedInMilliseconds = 0;
            var maxWaitTimeInMilliseconds = Settings.WaitForCompleteTimeOut * 1000;

            while (TestBrowser.IsAjaxRequestInProgress()
                    && timeWaitedInMilliseconds < maxWaitTimeInMilliseconds)
            {
                Thread.Sleep(Settings.SleepTime);
                timeWaitedInMilliseconds += Settings.SleepTime;
            }
        }

        public static bool IsAjaxRequestInProgress(this TestBrowser TestBrowser)
        {
            var evalResult = TestBrowser.Eval("watinAjaxMonitor.isRequestInProgress()");
            return evalResult == "true";
        }

        public static void InjectAjaxMonitor(this TestBrowser TestBrowser)
        {
            const string monitorScript =
                @"function AjaxMonitor(){"
                + "var ajaxRequestCount = 0;"

                + "$(document).ajaxStart(function(){"
                + "    ajaxRequestCount++;"
                + "});"

                + "$(document).ajaxComplete(function(){"
                + "    ajaxRequestCount--;"
                + "});"

                + "this.isRequestInProgress = function(){"
                + "    return (ajaxRequestCount > 0);"
                + "};"
                + "}"

                + "var watinAjaxMonitor = new AjaxMonitor();";

            TestBrowser.Eval(monitorScript);
        }
    }
}