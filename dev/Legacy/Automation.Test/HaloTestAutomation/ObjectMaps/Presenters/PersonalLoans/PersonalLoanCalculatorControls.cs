using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanCalculatorControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_tbLoanAmount_txtRands")]
        protected TextField txtLoanAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_tbLoanAmount_txtCents")]
        protected TextField txtLoanAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_tbLoanTerm")]
        protected TextField txtLoanTerm { get; set; }

        [FindBy(Id = "ctl00_Main_chkLifePolicy")]
        protected CheckBox chkLifePolicy { get; set; }

        [FindBy(Id = "ctl00_Main_btnCalculate")]
        protected Button btnCalculate { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnCreateApplication")]
        protected Button btnCreateApplication { get; set; }

        [FindBy(Id = "ctl00_Main_btnCreateApplication")]
        protected Button btnUpdateApplication { get; set; }

        [FindBy(Id = "ctl00_Main_grdLoanOptions")]
        protected Table grdLoanOptions { get; set; }

        [FindBy(Id = "ctl00_Main_lblMonthlyFee")]
        protected Span lblMonthlyFee { get; set; }

        [FindBy(Id = "ctl00_Main_lblInitiationFee")]
        protected Span lblInitiationFee { get; set; }

        [FindBy(Id = "ctl00_Main_lblLifeCreditpremium")]
        protected Span lblCreditLifePremium { get; set; }

        protected bool gridOptionExists(int term)
        {
            return this.grdLoanOptions.TableCells.Exists(Find.ByText(term.ToString()));
        }

        protected TableRow GetTableRowByTermValue(int term)
        {
            TableCellCollection options = this.grdLoanOptions.TableCells;
            return options.Filter(Find.ByText(term.ToString()))[0].ContainingTableRow;
        }
    }
}