using Common.Constants;
using System;
using System.Linq;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CATSDisbursementBaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblTransactionType")]
        protected Span lblTransactionType { get; set; }

        [FindBy(Id = "ctl00_Main_lblDisbursementsTotal")]
        protected Span lblDisbursementsTotal { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_grdDisbursements")]
        protected Table grdDisbursements { get; set; }

        protected TableRowCollection DisbursementBankAccountDetailRows
        {
            get
            {
                return grdDisbursements.TableRows;
            }
        }

        protected TableRow DisbursementBankAccountDetailRow(DateTime preparedDate, string acbBankDescription, string acbBranchCode, string acbBranchDescription, string acbTypeDescription, string accountNumber, string accountName, double amount)
        {
            string bankDetails = RemoveDoubleSpaces(string.Format(@"{0},{1},{2},{3},{4},{5}",
                     acbBankDescription,
                     acbBranchCode,
                     acbBranchDescription,
                     acbTypeDescription,
                     accountNumber,
                     accountName));
            string sAmount = RemoveDoubleSpaces(amount.ToString(Formats.CurrencyFormat2));
            var row = (from r in DisbursementBankAccountDetailRows
                       where r.TableCell(Find.ByText(preparedDate.ToString(Formats.DateFormat))).Exists
                       && r.TableCell(Find.ByText(bankDetails)).Exists
                       //&& r.TableCell(Find.ByText(sAmount)).Exists
                       select r).FirstOrDefault();

            return row;
        }

        protected string RemoveDoubleSpaces(string text)
        {
            string[] split = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;

            foreach (string s in split)
                result += s + " ";

            return result.TrimEnd(' ');
        }
    }
}