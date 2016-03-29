using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class ProposalService : _2AMDataHelper, IProposalService
    {
        /// <summary>
        /// Inserts an active proposal recrods against a debt counselling case
        /// </summary>
        /// <param name="debtCounsellingKey">Debt Counselling Case</param>
        /// <param name="numberOfProposalItems">No of proposal line items wanted</param>
        /// <param name="aduserName">ADUserName</param>
        /// <param name="hocInclusive">HOC Inclusive = 1 Exclusive = 0</param>
        /// <param name="lifeInclusive">Life Inclusive = 1 Exclusive = 0</param>
        public void InsertActiveProposal(int debtCounsellingKey, int numberOfProposalItems, string aduserName, int hocInclusive, int lifeInclusive, int monthlyServiceFeeInclusive)
        {
            base.InsertProposal(debtCounsellingKey, ProposalStatusEnum.Active, numberOfProposalItems, aduserName, hocInclusive, lifeInclusive, monthlyServiceFeeInclusive);
        }

        /// <summary>
        /// Gets the Proposal.ProposalKey for a debt counselling cases proposal when provided with a proposal status and an accepted flag.
        /// </summary>
        /// <param name="debtCounsellingKey">DebtCounsellingKey</param>
        /// <param name="accepted"></param>
        /// <param name="status"></param>
        /// <returns>Proposal.ProposalKey</returns>
        public int GetProposalKeyByStatusAndAcceptedStatus(int debtCounsellingKey, ProposalStatusEnum status, ProposalAcceptedEnum accepted, ProposalTypeEnum proposalType)
        {
            var proposalDetails = base.GetProposalDetails(debtCounsellingKey, status, accepted, proposalType);
            int proposalKey = (from p in proposalDetails select p.Column("ProposalKey").GetValueAs<int>()).FirstOrDefault();
            return proposalKey;
        }

        /// <summary>
        /// Updates the proposal end date
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="term"></param>
        public void UpdateProposalEndDate(int accountKey, int term)
        {
            base.UpdateProposalEndDate(accountKey, term, (int)ProposalTypeEnum.Proposal, (int)ProposalStatusEnum.Active);
        }

        /// <summary>
        /// Gets the Proposal.ReviewDate for the accepted proposal linked to our debt counselling case
        /// </summary>
        /// <param name="debtCounsellingKey">DebtCounsellingKey</param>
        /// <returns>Proposal.ProposalKey</returns>
        public string GetReviewDateOfAcceptedProposal(int debtCounsellingKey, ProposalTypeEnum proposalType)
        {
            return base.GetProposalDetails(debtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.True,
                proposalType).Rows(0).Column("ReviewDate").GetValueAs<string>();
        }

        /// <summary>
        /// Get a proposal
        /// </summary>
        /// <param name="debtcounsellingkey"></param>
        /// <returns></returns>
        public Automation.DataModels.Proposal GetProposal(int debtcounsellingkey, ProposalStatusEnum status, ProposalTypeEnum propType)
        {
            var proposals = base.GetProposals(debtcounsellingkey);
            var proposal = (from p in proposals
                            where p.DebtCounsellingKey == debtcounsellingkey
                                && p.ProposalStatusKey == status
                                && p.ProposalTypeKey == propType
                            select p).FirstOrDefault();

            var proposalItems = base.GetProposalItems(proposal.ProposalKey);
            proposal.ProposalItems = (from pItem in proposalItems
                                      select pItem).ToList();
            return proposal;
        }

        /// <summary>
        /// Get a proposal item
        /// </summary>
        /// <param name="proposalkey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Automation.DataModels.ProposalItem GetProposalItem(int proposalkey, DateTime startDate, DateTime endDate)
        {
            var proposalItems = base.GetProposalItems(proposalkey);
            return (from p in proposalItems
                    where p.ProposalKey == proposalkey
                        && p.StartDate.Date == startDate.Date
                        && p.EndDate.Date == endDate.Date
                    select p).FirstOrDefault();
        }

        /// <summary>
        /// Fethces the active recoveries proposal for the specified account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns></returns>
        public Automation.DataModels.RecoveriesProposal GetActiveRecoveriesProposalForAccount(int accountKey)
        {
            var recoveriesProposal = base.GetRecoveriesProposals();
            var proposal = (from r in recoveriesProposal
                            where r.AccountKey == accountKey && r.GeneralStatusKey == (int)GeneralStatusEnum.Active
                            select r).FirstOrDefault();
            return proposal;
        }
    }
}