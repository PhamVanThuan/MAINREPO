using WatiN.Core;

namespace ObjectMaps
{
    public class SAHLWebsiteCalculators
    {
        private Browser b;

        public SAHLWebsiteCalculators(Browser browser)
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

        public Link NewPurchaseCalculator
        {
            get
            {
                return b.Link(Find.ByText("New Home Purchase Mortgage Calculator"));
            }
        }
    }
}