using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class ConsultantDecline : ConsultantDeclineControls
    {
        /// <summary>
        /// Clicks the Submit button.
        /// </summary>
        public void ClickSubmit()
        {
            base.Submit.Click();
        }

        /// <summary>
        /// Identifies the proposal to decline in the table, selects it and clicks on the submit button.
        /// </summary>
        /// <param name="proposalType">Proposal Type</param>
        /// <param name="proposalStatus">Proposal Status</param>
        public void SelectProposalToDecline(string proposalType, string proposalStatus)
        {
            base.ProposalToDeclineTableRow(proposalType, proposalStatus).Click();
            ClickSubmit();
        }
    }
}