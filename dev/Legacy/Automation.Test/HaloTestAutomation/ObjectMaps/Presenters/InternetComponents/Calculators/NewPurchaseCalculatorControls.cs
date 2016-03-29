using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class NewPurchaseCalculatorControls : BaseCalculatorControls
    {
        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_tbPurchasePrice")]
        public TextField PurchasePrice { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_tbCashDeposit")]
        public TextField CashDeposit { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_tbHouseholdIncome")]
        public TextField HouseholdIncome { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_tbFixPercentage")]
        public TextField FixPercentage { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_tbLoanTerm")]
        public TextField LoanTerm { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_rbSalaried")]
        public RadioButton SalariedCheck { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_rbSelfEmployed")]
        public RadioButton SelfEmployedCheck { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_rbVariableRate")]
        public RadioButton VariableRateLoanCheck { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_rbFixedRate")]
        public RadioButton FixedRateLoanCheck { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_chkInterestOnly")]
        public CheckBox InterestOnlyCheck { get; set; }

        [FindBy(Name = "dnn$ctl01$lbCalculators$ctl02$calculatorMortgage$ctl17")]
        public Button Apply { get; set; }
    }
}