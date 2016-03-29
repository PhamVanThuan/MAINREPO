using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FurtherLendingCalculatorControls : BasePageControls
    {
        #region Buttons

        [FindBy(Id = "ctl00_Main_btnReset")]
        protected Button ResetButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCalculate")]
        protected Button CalculateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdateContact")]
        protected Button UpdateContactButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnGenerate")]
        protected Button GenerateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnBack")]
        protected Button BackButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnNext")]
        protected Button NextButton { get; set; }

        #endregion Buttons

        #region SelectLists

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_ddlRate")]
        protected SelectList RateDropdown { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_ddlEmploymentType")]
        protected SelectList EmploymentType { get; set; }

        #endregion SelectLists

        #region CheckBoxes

        [FindBy(Id = "ctl00_Main_tabsMenu_tpLoanCondition_chkCondition")]
        protected CheckBox ConditionCheckBox { get; set; }

        protected CheckBox SendToFaxCheckBox (int legalEntityIdentifier)
        {
            return base.Document.TableCell(Find.ByText(legalEntityIdentifier.ToString())).ParentTableRow.CheckBox(@"ctl00_Main_tabsMenu_tpInformation_LegalEntityGrid_ctl[0-9]*_Fax");
        }

        protected CheckBox SendToRow1FaxCheckBox
        {
            get
            {
                return base.Document.TableRow(@"ctl00$Main$tabsMenu$tpInformation$LegalEntityGrid_0").CheckBox(new Regex(@"ctl00_Main_tabsMenu_tpInformation_LegalEntityGrid_ctl[0-9]*_Fax"));
            }
        }

        protected CheckBox SendToEmailCheckBox(int legalEntityIdentifier)
        {
            return base.Document.TableCell(Find.ByText(legalEntityIdentifier.ToString())).ParentTableRow.CheckBox(@"ctl00_Main_tabsMenu_tpInformation_LegalEntityGrid_ctl[0-9]*_Email");
        }

        protected CheckBox SendToRow1EmailCheckBox
        {
            get
            {
                return base.Document.TableRow(@"ctl00$Main$tabsMenu$tpInformation$LegalEntityGrid_0").CheckBox(new Regex(@"ctl00_Main_tabsMenu_tpInformation_LegalEntityGrid_ctl[0-9]*_Email"));
            }
        }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpConfirmation_chkIncludeNaedoForm")]
        protected CheckBox IncludeNaedoForm { get; set; }

        #endregion CheckBoxes

        #region TextFields

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbReadvanceRequired")]
        protected TextField ReadvanceRequired { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbFurtherAdvReq")]
        protected TextField FurtherAdvReq { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbFurtherLoanReq")]
        protected TextField FurtherLoanReq { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbTotalCashRequired")]
        protected TextField TotalCashRequired { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbBondToRegister")]
        protected TextField BondToRegister { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbEstimatedValuationAmount")]
        protected TextField EstimatedValuationAmount { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbNewIncome1")]
        protected TextField NewIncome1 { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbReadvanceAccepted")]
        protected TextField ReadvanceAccepted { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbFurtherAdvanceAccepted")]
        protected TextField FurtherAdvanceAccepted { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbFurtherLoanAccepted")]
        protected TextField FurtherLoanAccepted { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbApprovalMode")]
        protected TextField ApprovalMode { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_tbProduct")]
        protected TextField Product { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpLoanCondition_tbSendApplication")]
        protected TextField SendApplication { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbHomePhone__CODE")]
        protected TextField HomePhoneCODE { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbHomePhone__NUMB")]
        protected TextField HomePhoneNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbWorkPhone__CODE")]
        protected TextField WorkPhoneCODE { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbWorkPhone__NUMB")]
        protected TextField WorkPhoneNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbFax__CODE")]
        protected TextField FaxCODE { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbFax__NUMB")]
        protected TextField FaxNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbCellNumber")]
        protected TextField CellNumber { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbEmail")]
        protected TextField Email { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbAlternateFaxNumber")]
        protected TextField AlternateFaxNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpInformation_tbAlternateEmail")]
        protected TextField AlternateEmail { get; set; }

        #endregion TextFields

        #region Labels

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblNewPTI")]
        public Span NewPTI { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblReAdvance")]
        public Span QualifyReadvance { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblFurtherAdvance")]
        public Span QualifyFurtherAdvance { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblFurtherLoan")]
        public Span QualifyFurtherLoan { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblHouseholdIncome")]
        public Span HouseholdIncome { get; set; }

        [FindBy(Id = "ctl00_Main_tabsMenu_tpCalculator_lblNewLTV")]
        public Span newLTV { get; set; }

        [FindBy(Id = "__tab_ctl00_Main_tabsMenu_tpVarifix")]
        public Span VarifixLabel { get; set; }

        public bool VariFixTabExists
        {
            get
            {
                return VarifixLabel.Exists;
            }
        }

        #endregion Labels
    }
}