using ObjectMaps.Pages;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Presenters.LoanServicing
{
    public abstract class TransactionRollBackControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnRollback")]
        protected Button btnRollback { get; set; }

        [FindBy(Id = "ctl00_Main_grdTransaction_DXMainTable")]
        protected Table LoanTransactions { get; set; }

        [FindBy(Class = "dxgvPagerBottomPanel_SoftOrange")]
        protected Div dxgvPagerBottomPanel { get; set; }

        protected TableCellCollection dxpPageNumber
        {
            get
            {
                return base.Document.TableCells.Filter(Find.ByClass(new Regex("^dxpPageNumber_SoftOrange[\x20-\x7E]*$")));
            }
        }

        protected TableCell dxpPageNumber_Current
        {
            get
            {
                return base.Document.TableCell(Find.ByClass(new Regex("dxpPageNumber_SoftOrange dxpCurrentPageNumber_SoftOrange")));
            }
        }

        [FindBy(Alt = "Prev")]
        protected Image Previous { get; set; }

        [FindBy(Alt = "Next")]
        protected Image Next { get; set; }

        [FindBy(Class = "dxWeb_pNextDisabled_SoftOrange")]
        protected Image DisabledNext { get; set; }

        protected bool TransactionExists(int financialTransactionKey)
        {
            return LoanTransactions.TableCells.Exists(Find.ByText(financialTransactionKey.ToString()));
        }

        protected TableRow SelectTransactionRow(int financialTransactionKey)
        {
            var transactions = LoanTransactions.TableCells;
            return transactions.Filter(Find.ByText(financialTransactionKey.ToString()))[0].ContainingTableRow;
        }
    }
}