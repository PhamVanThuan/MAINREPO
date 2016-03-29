using WatiN.Core;
using WatiN.Core.Exceptions;

namespace ObjectMaps.Pages
{
    public abstract class ManualDebitOrderBaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_RecordsGrid")]
        protected Table FutureDateTransactions { get; set; }

        [FindBy(Id = "ctl00_Main_lblBank")]
        protected Label BankLabel { get; set; }

        [FindBy(Id = "ctl00_Main_lblEffectiveDate")]
        protected Label EffectiveDateLabel { get; set; }

        [FindBy(Id = "ctl00_Main_lblAmount")]
        protected Label AmountLabel { get; set; }

        [FindBy(Id = "ctl00_Main_lblReference")]
        protected Label ReferenceLabel { get; set; }

        [FindBy(Id = "ctl00_Main_lblNote")]
        protected Span NoteText { get; set; }

        [FindBy(Id = "ctl00_Main_RecordsGridPrv")]
        protected Table PreviousTransactions { get; set; }

        [FindBy(Id = "ctl00_Main_lblArrearBalance")]
        protected Span ArrearBalanceLabel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlBankUpdate")]
        protected SelectList Bank { get; set; }

        [FindBy(Id = "ctl00_Main_dateEffectiveDateUpdate")]
        protected TextField EffectiveDate { get; set; }

        [FindBy(Id = "ctl00_Main_txtAmountUpdate_txtRands")]
        protected TextField AmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtAmountUpdate_txtCents")]
        protected TextField AmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtReferenceUpdate")]
        protected TextField Reference { get; set; }

        [FindBy(Id = "ctl00_Main_txtNoteUpdate")]
        protected TextField Note { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button Cancel { get; set; }

        /// <summary>
        /// return a row that contains the identifier provided in the future dated transactions grid
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        protected TableRow FutureDateTransactionsSelectRow(string identifier)
        {
            var cells = this.FutureDateTransactions.TableCells;
            if (cells.Filter(Find.ByText(identifier)).Count == 0)
                throw new WatiNException("No rows to select");
            return cells.Filter(Find.ByText(identifier))[0].ContainingTableRow;
        }

        /// <summary>
        /// return a row that contains the identifier provided in the previous transactions grid
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        protected TableRow PreviousTransactionsSelectRow(string identifier)
        {
            var cells = this.PreviousTransactions.TableCells;
            return cells.Filter(Find.ByText(identifier))[0].ContainingTableRow;
        }
    }
}