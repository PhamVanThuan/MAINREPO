using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteCalculatorControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteCalculatorControls(Browser browser)
        {
            b = browser;
        }

        public Link SwitchCalculator
        {
            get
            {
                return b.Link(Find.ByText("Switch your Home Loan Calculator"));
            }
        }

        public Link AffordabilityCalculator
        {
            get
            {
                return b.Link(Find.ByText("Affordability Calculator"));
            }
        }

        public Link NewPurchaseCalculator
        {
            get
            {
                return b.Link(Find.ByText("New Home Purchase Mortgage Calculator"));
            }
        }
    }
}