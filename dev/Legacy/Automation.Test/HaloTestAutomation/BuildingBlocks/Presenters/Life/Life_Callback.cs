using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Callback : LifeCallbackControls
    {
        /// <summary>
        /// Creates a Callback for a life workflow case
        /// </summary>
        /// <param name="CallBackReason">Reason</param>
        /// <param name="Date">Date</param>
        /// <param name="Note">Note to Capture</param>
        public void CreateCallBack(string CallBackReason, DateTime Date, string Note)
        {
            base.ctl00MainddlReason.Option(CallBackReason).Select();
            string _date = Date.Day + "/" + Date.Month + "/" + Date.Year;
            base.ctl00MaindteCallbackDate.Value = _date;
            base.ctl00MaintxtNotes.TypeText(Note);
            base.ctl00MaindteCallbackDate.Focus();
            base.ctl00MainbtnSubmit.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}