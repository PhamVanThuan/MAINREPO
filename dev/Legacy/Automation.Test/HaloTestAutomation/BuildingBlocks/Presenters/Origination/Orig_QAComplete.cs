using ObjectMaps;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class Orig_QAComplete : Orig_QAQueryControls
    {
        public void QAComplete(string Reason)
        {
            base.AvailableReasons.Option(Reason).Select();
            base.btnAdd.Click();
            CommonIEDialogHandler.SelectOK(base.btnConfirm);
        }
    }
}