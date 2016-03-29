using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class TermReviewUpdate : TermReviewUpdateControls
    {
        /// <summary>
        /// Enters a review date more than 18 months in the future
        /// </summary>
        public void EnterReviewDateGreaterThan18Months()
        {
            var dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddMonths(19);
            base.txtReviewDate.Value = dt.ToString(Formats.DateFormat);
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Enters a review date less than today
        /// </summary>
        public void EnterReviewDateLessThanToday()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddDays(-1);
            base.txtReviewDate.Value = dt.ToString(Formats.DateFormat);
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Enters a valid review date
        /// </summary>
        public string EnterValidReviewDate()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddMonths(6);
            base.txtReviewDate.Value = dt.ToString(Formats.DateFormat);
            base.btnUpdate.Click();
            return dt.ToString(Formats.DateFormat);
        }

        /// <summary>
        ///
        /// </summary>
        public void EnterBlankReviewDate()
        {
            base.txtReviewDate.Clear();
            base.btnUpdate.Click();
        }
    }
}