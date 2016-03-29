using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public abstract class ProposalsNodeControls : BaseNavigation
    {
        [FindBy(Text = "Proposals")]
        protected Link linkProposals { get; set; }

        [FindBy(Text = "Proposal")]
        protected Link linkProposalSummary { get; set; }

        [FindBy(Text = "Manage Proposals")]
        protected Link linkManageProposal { get; set; }

        [FindBy(Text = "Manage Counter Proposals")]
        protected Link linkManageCounterProposals { get; set; }
    }
}