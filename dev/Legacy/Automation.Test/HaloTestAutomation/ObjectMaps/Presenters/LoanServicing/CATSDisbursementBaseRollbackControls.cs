using Common.Constants;
using System;
using System.Linq;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CATSDisbursementRollbackControls : CATSDisbursementBaseControls
    {
        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnRollback { get; set; }

        [FindBy(Id = "ctl00_Main_grdLoanTransactions")]
        protected Table LoanTransactionGrid { get; set; }

        protected TableRowCollection LoanTransactionRows
        {
            get
            {
                return LoanTransactionGrid.TableRows;
            }
        }

        protected TableRow LoanTransactionRow(int? financialTransactionKey)
        {
            var row = (from lt in LoanTransactionRows
                       where lt.TableCell(Find.ByText(financialTransactionKey.ToString())).Exists
                       select lt).FirstOrDefault<TableRow>();
            return row;
        }

        [FindBy(Id = "ctl00_Main_grdDisbursementTransactions")]
        protected Table DisbursementTransactionsGrid { get; set; }

        protected TableRowCollection DisbursementTransactionsRows
        {
            get
            {
                return DisbursementTransactionsGrid.TableRows;
            }
        }

        protected TableRow DisbursementTransactionsRow(DateTime preparedDate, string acbBankDescription, string acbBranchCode, string acbBranchDescription, string acbTypeDescription, string accountNumber, string accountName, double amount)
        {
            string bankDetails = RemoveDoubleSpaces(string.Format(@"{0} - {1} - {2} - {3} - {4} - {5}",
                        acbBankDescription,
                        acbBranchCode,
                        acbBranchDescription,
                        acbTypeDescription,
                        accountNumber,
                        accountName));

            var row = (from r in DisbursementTransactionsRows
                       where r.TableCell(Find.ByText(string.Format(Formats.DateFormat))).Exists && r.TableCell(Find.ByText(bankDetails)).Exists
                       select r).FirstOrDefault<TableRow>();

            return row;
        }
    }
}