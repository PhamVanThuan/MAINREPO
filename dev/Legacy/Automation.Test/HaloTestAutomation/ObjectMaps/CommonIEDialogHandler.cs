using WatiN.Core.DialogHandlers;
using System.Linq;

namespace ObjectMaps
{
    public static class CommonIEDialogHandler
    {
        public static void SelectOK(WatiN.Core.Button DialogTriggerButton)
        {
            ConfirmDialogHandler handler = new ConfirmDialogHandler();
            using (new UseDialogOnce(DialogTriggerButton.DomContainer.DialogWatcher, handler))
            {
                DialogTriggerButton.ClickNoWait();
                handler.WaitUntilExists();
                handler.OKButton.Click();
            }
            DialogTriggerButton.WaitForComplete();
        }

        public static string SelectOKInAlertDialogAndReturnMessage(WatiN.Core.Button DialogTriggerButton)
        {
            string message = string.Empty;
            AlertDialogHandler handler = new AlertDialogHandler();
            using (new UseDialogOnce(DialogTriggerButton.DomContainer.DialogWatcher, handler))
            {
                DialogTriggerButton.ClickNoWait();
                handler.WaitUntilExists();
                message = handler.Message;
                handler.OKButton.Click();
            }
            return message;
        }
    }
}