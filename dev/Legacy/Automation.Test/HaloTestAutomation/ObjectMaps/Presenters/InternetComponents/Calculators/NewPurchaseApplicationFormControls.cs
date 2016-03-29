using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class NewPurchaseApplicationFormControls : BaseCalculatorControls
    {
        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_txtFirstNames")]
        public TextField FirstNames { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_txtSurname")]
        public TextField Surname { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_phCode")]
        public TextField PhoneCode { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_phNumber")]
        public TextField PhoneNumber { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_tbNumApplicants")]
        public TextField NumApplicants { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_txtEmailAddress")]
        public TextField EmailAddress { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_bttnApply")]
        public Button SumitApplication { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorMortgage_applicationCalculator_summaryApplication_lblSummaryReferenceNumber")]
        public Span ReferenceNumber { get; set; }
    }
}