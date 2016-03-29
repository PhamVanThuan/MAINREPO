using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DisbursementRollbackControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_grdLoanTransactions")]
        protected Table LoanTransactionsGrid { get; set; }

        protected TableRow gridSelectLoanDisbursementRecord(string CellValue)
        {
            TableCellCollection lt = LoanTransactionsGrid.TableCells;
            return lt.Filter(Find.ByText(CellValue))[0].ContainingTableRow;
        }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnRollback { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }
    }
}