using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class CalculatorsNode : MenuControls
    {
        public void Calculators(TestBrowser TestBrowser, CalculatorNodeTypeEnum CalculatorNode)
        {
            TestBrowser.Navigate<MenuNode>().Calculators();

            switch (CalculatorNode)
            {
                case CalculatorNodeTypeEnum.ApplicationCalculator:
                    base.linkApplicationCalculator.Click();
                    break;

                case CalculatorNodeTypeEnum.ApplicationWizard:
                    base.linkApplicationWizard.Click();
                    break;

                case CalculatorNodeTypeEnum.LeadCapture:
                    base.linkLeadCapture.Click();
                    break;

                case CalculatorNodeTypeEnum.CreateApplication:
                    base.linkCreateApplication.Click();
                    break;
            }
        }
    }
}