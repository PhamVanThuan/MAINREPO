using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO.PersonalLoan
{
    public sealed class ExternalLifeNode : ExternalLifeNodeControls
    {
        public void ClickLifeNode()
        {
            base.Life.Click();
        }

        public void ClickLifePolicySummaryNode()
        {
            base.LifePolicySummary.Click();
        }

        public void ClickUpdateExternalLifePolicyNode()
        {
            base.UpdateExternalLifePolicy.Click();
        }
    }
}