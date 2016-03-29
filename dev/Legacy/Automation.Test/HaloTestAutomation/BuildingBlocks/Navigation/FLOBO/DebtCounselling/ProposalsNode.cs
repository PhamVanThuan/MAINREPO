using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO.DebtCounselling
{
    public class ProposalsNode : ProposalsNodeControls
    {
        public void Proposals()
        {
            base.linkProposals.Click();
        }

        public void ProposalSummary()
        {
            base.linkProposalSummary.Click();
        }

        public void ManageProposal()
        {
            base.linkManageProposal.Click();
        }

        public void ManageCounterProposals()
        {
            base.linkManageCounterProposals.Click();
        }
    }
}