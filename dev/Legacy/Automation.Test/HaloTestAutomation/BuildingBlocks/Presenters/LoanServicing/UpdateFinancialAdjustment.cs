using Common.Enums;
using ObjectMaps.Pages;
using System.Linq;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing
{
    /// <summary>
    ///
    /// </summary>
    public class UpdateFinancialAdjustment : UpdateFinancialAdjustmentControls
    {
        /// Set the Financial Adjustment Status in the grid
        /// </summary>
        /// <param name="financialAdjustmentType"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        public void SetFinancialAdjustmentStatus(string financialAdjustmentType, FinancialAdjustmentStatusEnum existingStatus, string fromDate,
            FinancialAdjustmentStatusEnum newStatus)
        {
            //Find the row with the financial adjustment
            var row = FindFinancialAdjustmentTypeRowInTable(financialAdjustmentType, existingStatus.ToString(), fromDate);
            row.Click();
            Thread.Sleep(1000);
            //Activate the drop down list for the User status
            base.FinancialAdjustmentStatusDropDownList.MouseDown();
            Thread.Sleep(1000);
            switch (newStatus)
            {
                case FinancialAdjustmentStatusEnum.Active:
                    base.ActiveFinancialAdjustmentStatusCell.MouseUp();
                    break;

                case FinancialAdjustmentStatusEnum.Canceled:
                    base.CancelledFinancialAdjustmentStatusCell.MouseUp();
                    break;
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Click the Submit Button
        /// </summary>
        public void Submit()
        {
            base.UpdateButton.Click();
        }

        /// <summary>
        /// Click the Cancel Button
        /// </summary>
        public void Cancel()
        {
            base.CancelButton.Click();
        }

        /// <summary>
        /// Find the specific Rateoverride in the table
        /// </summary>
        /// <param name="fadjTypeDesc"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private TableRow FindFinancialAdjustmentTypeRowInTable(string fadjTypeDesc,
            string status, string fromDate)
        {
            var statusCell = default(TableCell);
            var effectiveDateCell = default(TableCell);
            var fadjTypeDescCell = default(TableCell);
            foreach (var tableRow in base.FinancialAdjustmentTable.TableRows)
            {
                fadjTypeDescCell = (from c1 in tableRow.OwnTableCells.Cast<TableCell>()
                                    where c1.Text == fadjTypeDesc
                                    select c1).FirstOrDefault();

                if (fadjTypeDescCell != null)
                    return tableRow;
            }
            return null;
        }
    }
}