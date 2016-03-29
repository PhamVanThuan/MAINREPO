using NUnit.Framework;
using ObjectMaps.Pages;
using System.Threading;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class CourtDetailsAdd : CourtDetailsAddControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="court">Court Details</param>
        public void AddCourtDetails(Automation.DataModels.CourtDetails court)
        {
            base.HearingType.Option(court.hearingType).Select();
            Thread.Sleep(2500);
            base.AppearanceType.Option(court.appearanceType).Select();
            //we only add a court if the Hearing Type = Court
            if (court.hearingType == Common.Constants.HearingType.Court)
            {
                base.Court.TypeText(court.court);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                base.SAHLAutoComplete_DefaultItem(court.court).MouseDown();
            }
            base.HearingDate.Value = court.hearingDate;
            base.CaseNumber.Value = court.caseNumber;
            if (!string.IsNullOrEmpty(court.comments))
                base.Comments.Value = court.comments;
            base.btnAdd.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="court">CourtDetails</param>
        /// <param name="b">IE TestBrowser Object</param>
        public void AddCourtDetailsNoCaseNumber(Automation.DataModels.CourtDetails court)
        {
            base.HearingType.Option(court.hearingType).Select();
            Thread.Sleep(2500);
            base.AppearanceType.Option(court.appearanceType).Select();
            //we only add a court if the Hearing Type = Court
            if (court.hearingType == Common.Constants.HearingType.Court)
            {
                base.Court.TypeText(court.court);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                base.SAHLAutoComplete_DefaultItem(court.court).MouseDown();
            }
            base.HearingDate.Value = court.hearingDate;
            base.Comments.Value = court.comments;
            base.btnAdd.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="court">CourtDetails</param>
        /// <param name="b">IE TestBrowser Object</param>
        public void AddCourtDetailsNoHearingDate(Automation.DataModels.CourtDetails court)
        {
            base.HearingType.Option(court.hearingType).Select();
            Thread.Sleep(2500);
            base.AppearanceType.Option(court.appearanceType).Select();
            //we only add a court if the Hearing Type = Court
            if (court.hearingType == Common.Constants.HearingType.Court)
            {
                base.Court.TypeText(court.court);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                base.SAHLAutoComplete_DefaultItem(court.court).MouseDown();
            }
            base.CaseNumber.Value = court.caseNumber;
            base.Comments.Value = court.comments;
            base.btnAdd.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="court">CourtDetails</param>
        /// <param name="b">IE TestBrowser Object</param>
        public void AddCourtDetailsNoCourt(Automation.DataModels.CourtDetails court)
        {
            base.HearingType.Option(court.hearingType).Select();
            Thread.Sleep(2500);
            base.AppearanceType.Option(court.appearanceType).Select();
            base.HearingDate.Value = court.hearingDate;
            base.CaseNumber.Value = court.caseNumber;
            base.Comments.Value = court.comments;
            base.btnAdd.Click();
        }

        /// <summary>
        /// Clears values that have been entered.
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        public void ClearCourtDetails()
        {
            base.Court.Clear();
            base.CaseNumber.Clear();
            base.Comments.Clear();
            base.HearingDate.Clear();
        }

        public void SelectAppearanceStatus(string appearanceStatus)
        {
            base.HearingType.Option("Court").Select();
            Thread.Sleep(2500);
            base.AppearanceType.Option(appearanceStatus).Select();
        }

        public void AssertLabelText(string expectedResult)
        {
            Logger.LogAction("Asserting that the following text exists in the Browser Window: {0}", expectedResult);
            Assert.That(Date.Text == expectedResult, "Expected result did not match the element found");
        }
    }
}