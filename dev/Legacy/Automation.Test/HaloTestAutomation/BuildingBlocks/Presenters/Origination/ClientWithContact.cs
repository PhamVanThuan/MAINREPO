using Common.Constants;
using ObjectMaps.Pages;
using System;
namespace BuildingBlocks.Presenters.Origination
{
    public class ClientWithContact : ClientWithContactControls
    {
        public void PopulateFields(string endDate, string comment)
        {
            this.ClearAll();
            base.ContactDate.Value = endDate;
            base.Comments.TypeText(comment);
        }
        public void ClickSubmit()
        {
            base.SubmitButton.Click();
        }
        public void ClickCancel()
        {
            base.CancelButton.Click();
        }
        public void ClearAll()
        {
            base.ContactDate.Clear();
            base.Comments.Clear();
        }
    }
}
