using WatiN.Core;

namespace ObjectMaps.NavigationControls.InternetComponents
{
    public class WebsiteCalculatorControls : BaseNavigation
    {
        [FindBy(Text = "Switch your Home Loan Calculator")]
        public Link SwitchCalculator { get; set; }

        [FindBy(Text = "Affordability Calculator")]
        public Link AffordabilityCalculator { get; set; }

        [FindBy(Text = "New Home Purchase Mortgage Calculator")]
        public Link NewPurchaseCalculator { get; set; }

        [FindBy(Id = "calculators-menu")]
        public Link Calculators { get; set; }

        [FindBy(Id = "dnn_ctl01_lbCalculators_ctl02_calculatorAffordability_applicationAffordability_summaryApplication_lblSummaryReferenceNumber")]
        public Span ReferenceNumber { get; set; }
    }
}