using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class AffordabilityCalculatorControls : BaseCalculatorControls
    {
        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtSalaryOne")]
        public TextField SalaryOne { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtSalaryTwo")]
        public TextField SalaryTwo { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtProfitFromSale")]
        public TextField ProfitFromSale { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtOtherContribution")]
        public TextField OtherContribution { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtMonthlyInstalment")]
        public TextField MonthlyInstalment { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtLoanTerm")]
        public TextField LoanTermYears { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_txtInterestRate")]
        public TextField InterestRate { get; set; }

        [FindBy(Name = "dnn$ctl01$lbCalculators$ctl02$calculatorAffordability$ctl14")]
        public Button Apply { get; set; }
    }
}