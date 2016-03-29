using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LoanCalculatorLeadControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlPurpose")]
        protected SelectList selectLoanPurpose { get; set; }

        [FindBy(Id = "ctl00_Main_ddlProduct")]
        protected SelectList selectProduct { get; set; }

        [FindBy(Id = "ctl00_Main_tbMarketValue_txtRands")]
        protected TextField textfieldMarketValueOfYourHome { get; set; }

        [FindBy(Id = "ctl00_Main_tbCurrentLoan_txtRands")]
        protected TextField textfieldCurrentLoanAmount { get; set; }

        [FindBy(Id = "ctl00_Main_tbCashOut_txtRands")]
        protected TextField textfieldCashOut { get; set; }

        [FindBy(Id = "ctl00_Main_tbPurchasePrice_txtRands")]
        protected TextField textfieldPurchasePrice { get; set; }

        [FindBy(Id = "ctl00_Main_tbCashDeposit_txtRands")]
        protected TextField textfieldCashDeposit { get; set; }

        [FindBy(Id = "ctl00_Main_tbCashRequired_txtRands")]
        protected TextField textfieldCashRequired { get; set; }

        [FindBy(Id = "ctl00_Main_ddlEmploymentType")]
        protected SelectList selectEmploymentType { get; set; }

        [FindBy(Id = "ctl00_Main_tbLoanTerm")]
        protected TextField textfieldTermOfLoan { get; set; }

        [FindBy(Id = "ctl00_Main_chkCapitaliseFees")]
        protected CheckBox checkboxCapitaliseFees { get; set; }

        //[FindBy(Id = "ctl00_Main_tbFixPercentage")]
        //protected TextField textfieldPercentageToFix { get; set; }

        [FindBy(Id = "ctl00_Main_tbHouseholdIncome_txtRands")]
        protected TextField textfieldGrossMonthlyHousholdIncome { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNeedsIdentification")]
        protected SelectList ddlNeedsIdentification { get; set; }

        [FindBy(Id = "ctl00_Main_btnCalculate")]
        protected Button btnCalculate { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnCreateApplication")]
        protected Button btnCreateApplication { get; set; }

        [FindBy(Id = "ctl00_Main_chkEstateAgent")]
        protected CheckBox chkEstateAgent { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMarketingSource")]
        protected SelectList ddlMarketingSource { get; set; }
    }
}