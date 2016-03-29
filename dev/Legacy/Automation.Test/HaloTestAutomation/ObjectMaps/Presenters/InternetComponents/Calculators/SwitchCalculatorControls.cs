using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class SwitchCalculatorControls : BaseCalculatorControls
    {
        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_tbMarketValue")]
        public TextField MarketValue { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_tbCurrentLoan")]
        public TextField CurrentLoanAmount { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_tbCashOut")]
        public TextField CashOut { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_rbFixedRate")]
        public TextField FixPercentage { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_tbLoanTerm")]
        public TextField LoanTerm { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_rbSalaried")]
        public RadioButton SalariedCheck { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_rbSelfEmployed")]
        public RadioButton SelfEmployedCheck { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_rbVariableRate")]
        public RadioButton VariableRateLoanCheck { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_rbFixedRate")]
        public RadioButton FixedRateLoanCheck { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_tbHouseholdIncome")]
        public TextField HouseholdIncome { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_chkInterestOnly")]
        public CheckBox InterestOnlyCheck { get; set; }

        [FindBy(Id = "dnn_ctl00_lbCalculators_ctl02_calculatorSwitch_chkCapitaliseFees")]
        public CheckBox CapitaliseFeesCheck { get; set; }

        [FindBy(Id = "dnn$ctl00$lbCalculators$ctl02$calculatorSwitch$ctl17")]
        public Button Apply { get; set; }
    }
}