using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class CalculatorApplicationFormControls : BaseCalculatorControls
    {
        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_txtFirstNames")]
        public TextField FirstNames { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_txtSurname")]
        public TextField Surname { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_phCode")]
        public TextField PhoneCode { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_phNumber")]
        public TextField PhoneNumber { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_tbNumApplicants")]
        public TextField NumApplicants { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_txtEmailAddress")]
        public TextField EmailAddress { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_bttnApply")]
        public Button SumitApplication { get; set; }
    }
}