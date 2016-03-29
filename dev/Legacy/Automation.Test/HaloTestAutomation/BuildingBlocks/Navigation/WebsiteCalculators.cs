using System;

namespace BuildingBlocks.Navigation.InternetComponents
{
    public class WebsiteCalculators : ObjectMaps.NavigationControls.InternetComponents.WebsiteCalculatorControls
    {
        public void ClickCalculators()
        {
            base.Calculators.Click();
        }

        public void ClickAffordabilityCalculator()
        {
            base.AffordabilityCalculator.Click();
        }

        public void ClickNewPurchaseCalculator()
        {
            base.NewPurchaseCalculator.Click();
        }

        public void ClickSwitchCalculator()
        {
            base.SwitchCalculator.Click();
        }

        public int GetOfferKey()
        {
            return Convert.ToInt32(base.ReferenceNumber.Text);
        }
    }
}