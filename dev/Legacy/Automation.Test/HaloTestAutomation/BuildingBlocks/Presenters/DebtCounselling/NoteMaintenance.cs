using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Globalization;
using System.Linq;
using System.Linq;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class NoteMaintenance : NoteMaintenanceControls
    {
        public void PopulateNoteDetails(string noteText = null, string tag = null)
        {
            var element = base.ReturnNativeHtmlElementByClass(base.Document.NativeDocument, "dxheDesignViewArea dxheViewArea");
            element.innerText = noteText;
            base.Tag.Value = tag;
        }

        public void PopulateDiaryDate(DateTime diaryDate)
        {
            base.DiaryDate.Value = diaryDate.ToString(Formats.DateFormat, DateTimeFormatInfo.CurrentInfo);
        }

        public void ClickSaveDiaryDateButton()
        {
            base.SaveDiaryDate.Click();
        }

        public void ClickCheckDiaryDateButton()
        {
            base.CheckDiaryDate.Click();
        }

        public void ClickAddNoteDetails()
        {
            base.Add.Click();
        }

        public void ClickUpdateAddNoteDetails()
        {
            base.Update.Click();
        }

        public void ClickCancelAddNoteDetails()
        {
            base.Cancel.Click();
        }

        public void AssertNoteRequiredMessage()
        {
            string html = base.Document.Html;
            string expectedTag = @"<TD class=dxgv style=""BORDER-RIGHT-WIDTH: 0px"" colSpan=4>Note is required.</TD></TR>";
            Assert.That(html.Contains(expectedTag));
        }

        public void AssertDiaryDateNotInPastMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Diary Date cannot be before today."));
        }

        public void AssertNoDiaryDateToCheckMessage()
        {
            Assert.True(base.ctl00_Main_lblRelatedEntries.Text.Contains("No Diary Date to check"), "Expected \"No Diary Date to check\" validation message");
        }

        public void AssertNoteTextDisabled()
        {
            var body = base.ReturnNativeHtmlElementByClass(base.Document.NativeDocument, "dxhePreviewArea dxheViewArea");
            string contentEditableAtt = body.getAttribute("contentEditable").ToString();
            Assert.AreNotEqual("true", contentEditableAtt);
        }

        public void ClickEditNoteDetails()
        {
            base.Edit.Click();
        }

        public void AssertCheckDiaryMessage(int applicationCount, string aduser, DateTime diaryDate)
        {
            aduser = aduser.Replace("\\", "\\\\");
            var date = diaryDate.Date.ToString(Formats.DateFormat);
            var message = String.Empty;
            if (applicationCount == 0)
                message = String.Format("There are no Applications diarised for {0} on {1}", aduser, date);
            else
                message = String.Format("There are {0} Applications diarised for {1} on {2}", applicationCount, aduser, date);
            Assert.True(base.ctl00_Main_lblRelatedEntries.Text.Contains(message), "Expected message:{0}", message);
        }

        public void ClearDiaryDate()
        {
            base.DiaryDate.TypeText(String.Empty);
        }
    }
}