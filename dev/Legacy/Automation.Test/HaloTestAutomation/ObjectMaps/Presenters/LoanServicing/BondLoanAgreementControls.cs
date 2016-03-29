using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class BondLoanAgreementControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_LoanAgreeDateAdd")]
        protected TextField txtLoanAgreementDate { get; set; }

        [FindBy(Id = "ctl00_Main_LoanAgreeAmountAdd")]
        protected TextField txtLoanAgreementAmount { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_BondGrid")]
        protected Table LoanAgreementGrid { get; set; }

        protected TableRow LoanAgreementGridSelectRow(string bondRegNumber)
        {
            TableCellCollection laa = LoanAgreementGrid.TableCells;
            return laa.Filter(Find.ByText(bondRegNumber))[0].ContainingTableRow;
        }

        [FindBy(Id = "ctl00_Main_DeedsOfficeUpdate")]
        protected SelectList ddlDeedsOffice { get; set; }

        [FindBy(Id = "ctl00_Main_AttorneyUpdate")]
        protected SelectList ddlAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_BondRegNumberUpdate")]
        protected TextField BondRegistrationNumber { get; set; }

        [FindBy(Id = "ctl00_Main_BondRegAmountUpdate")]
        protected TextField BondRegistrationAmount { get; set; }

        [FindBy(Id = "ctl00_Main_BondRegDate")]
        protected Span BondRegistrationDate { get; set; }
    }
}