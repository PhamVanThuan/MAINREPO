using WatiN.Core;

namespace ObjectMaps.Pages
{
    ///<summary>
    /// Contains the Object Mapping for the Affordability Details Screens
    ///</summary>
    public abstract class AffordabilityDetailsControls : BasePageControls
    {
        // Income
        [FindBy(Id = "ctl00_Main_txtDependantsInHousehold")]
        protected TextField txtDependantsInHousehold { get; set; }

        [FindBy(Id = "ctl00_Main_txtContributingDependants")]
        protected TextField txtContributingDependants { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl0_txtAmount_txtRands")]
        protected TextField txtBasic { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl1_txtAmount_txtRands")]
        protected TextField txtCommission { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl2_txtAmount_txtRands")]
        protected TextField txtRentalIncome { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl3_txtAmount_txtRands")]
        protected TextField txtIncomeOtherInv { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl3_txtDescription")]
        protected TextField txtIncomeOtherInvestmentsDesc { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl4_txtAmount_txtRands")]
        protected TextField txtOtherIncome1 { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl4_txtDescription")]
        protected TextField txtOtherIncome1Desc { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl5_txtAmount_txtRands")]
        protected TextField txtOtherIncome2 { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_ctrl5_txtDescription")]
        protected TextField txtOtherIncome2Desc { get; set; }

        // Monthly Expenses
        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl0_txtAmount_txtRands")]
        protected TextField txtSalaryDeductions { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl1_txtAmount_txtRands")]
        protected TextField txtLivingExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl12_txtAmount_txtRands")]
        protected TextField txtOtherExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl12_txtDescription")]
        protected TextField txtOtherExpensesDesc { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl2_txtAmount_txtRands")]
        protected TextField txtMedicalExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl3_txtAmount_txtRands")]
        protected TextField txtClothing { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl4_txtAmount_txtRands")]
        protected TextField txtUtilities { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl5_txtAmount_txtRands")]
        protected TextField txtRatesAndTaxs { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl6_txtAmount_txtRands")]
        protected TextField txtTransport { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl7_txtAmount_txtRands")]
        protected TextField txtInsurance { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl8_txtAmount_txtRands")]
        protected TextField txtDomestic { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl9_txtAmount_txtRands")]
        protected TextField txtTelephone { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl10_txtAmount_txtRands")]
        protected TextField txtEducation { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_ctrl11_txtAmount_txtRands")]
        protected TextField txtChildSupport { get; set; }

        // Debt Payment
        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl0_txtAmount_txtRands")]
        protected TextField txtBondRepayments { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl1_txtAmount_txtRands")]
        protected TextField txtRentalPayments { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl2_txtAmount_txtRands")]
        protected TextField txtCarRepayments { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl3_txtAmount_txtRands")]
        protected TextField txtCreditCards { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl4_txtAmount_txtRands")]
        protected TextField txtOverdraft { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl5_txtAmount_txtRands")]
        protected TextField txtPersonalLoans { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl6_txtAmount_txtRands")]
        protected TextField txtRetailAccounts { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl7_txtAmount_txtRands")]
        protected TextField txtCreditAccounts { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl8_txtAmount_txtRands")]
        protected TextField txtPlannedSavings { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl9_txtAmount_txtRands")]
        protected TextField txtOtherInstalments { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl9_txtDescription")]
        protected TextField txtOtherInstalmentsDesc { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl10_txtAmount_txtRands")]
        protected TextField txtOtherDebtPayment { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_ctrl10_txtDescription")]
        protected TextField txtOtherDebtPaymentDesc { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnSubmit { get; set; }

        [FindBy(Id = "ctl00_Main_IncomeListView_lblTotal")]
        protected Span lblIncomeTotal { get; set; }

        [FindBy(Id = "ctl00_Main_MonthlyExpenseListView_lblTotal")]
        protected Span lblMonthlyExpenseTotal { get; set; }

        [FindBy(Id = "ctl00_Main_DebtRepaymentListView_lblTotal")]
        protected Span lblDebtRepaymentTotal { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalIncome")]
        protected Span lblTotalIncome { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalExpenses")]
        protected Span lblTotalExpenses { get; set; }

        [FindBy(Id = "ctl00_Main_lblAfordability")]
        protected Span lblAffordability { get; set; }
    }
}