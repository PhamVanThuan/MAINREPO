using ObjectMaps;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class Orig_QAQuery : Orig_QAQueryControls
    {
        /// <summary>
        /// Completes the QA Query activity
        /// </summary>
        /// <param name="Reason"></param>
        public void SubmitQAQuery(string Reason)
        {
            base.AvailableReasons.Option(Reason).Select();
            base.btnAdd.Click();
            CommonIEDialogHandler.SelectOK(base.btnConfirm);
        }
    }
}