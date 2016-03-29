using WatiN.Core.DialogHandlers;

namespace BuildingBlocks
{
    public sealed class TestBrowserLoginHandle : WaitUntilHandledDialogHandler
    {
        private string username;
        private string password;
        private string url;

        public TestBrowserLoginHandle(string username, string password, string url)
        {
            this.username = username;
            this.password = password;
            this.url = url;
        }

        public override bool CanHandleDialog(WatiN.Core.Native.Windows.Window window)
        {
            return true;
        }

        public override bool HandleDialog(WatiN.Core.Native.Windows.Window window)
        {
            Win7BrowserLogin.IELoginHandler.LoginToBrowser(window.ParentHwnd, username, password, url);
            return true;
        }
    }
}