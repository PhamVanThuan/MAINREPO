using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.LoanDetail
{
    public class LoanDetailBase : LoanDetailBaseControls
    {
        /// <summary>
        /// Adds the detail date.
        /// </summary>
        /// <param name="date">Detail Date</param>
        public void AddDetailDate(DateTime date)
        {
            base.DetailDate.Value = date.ToString(Formats.DateFormat);
        }

        /// <summary>
        /// Selects the detail type in the Loan Detail grid.
        /// </summary>
        /// <param name="detailType">Detail Type</param>
        public void SelectDetailType(string detailType)
        {
            var row = base.LoanDetail.FindRowInOwnTableRows(detailType, 3);
            if (row != null)
                row.Click();
        }

        /// <summary>
        /// Clicks the cancel button.
        /// </summary>
        public void ClickCancel()
        {
            base.Cancel.Click();
        }
    }
}