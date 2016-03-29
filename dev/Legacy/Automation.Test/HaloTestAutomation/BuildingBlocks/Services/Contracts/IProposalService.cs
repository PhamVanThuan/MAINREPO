using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IProposalService
    {
        void InsertActiveProposal(int debtCounsellingKey, int numberOfProposalItems, string aduserName, int hocInclusive, int lifeInclusive, int monthlyServiceFeeInclusive);

        void InsertProposal(int debtCounsellingKey, ProposalStatusEnum proposalStatusKey, int numberOfProposalItems, string aduserName, int hocInclusive, int lifeInclusive);

        int GetProposalKeyByStatusAndAcceptedStatus(int debtCounsellingKey, ProposalStatusEnum status, ProposalAcceptedEnum accepted, ProposalTypeEnum proposalType);

        void UpdateProposalEndDate(int accountKey, int term);

        string GetReviewDateOfAcceptedProposal(int debtCounsellingKey, ProposalTypeEnum proposalType);

        void InsertCounterProposalByStatus(int debtCounsellingKey, ProposalStatusEnum proposalStatusKey, int proposalItems);

        void DeleteProposal(int debtCounsellingKey, ProposalTypeEnum proposalTypeKey, ProposalStatusEnum proposalStatusKey);

        List<int> GetAllProposalsAndCounterProposalsByDebtCounsellingKey(int debtCounsellingKey);

        DateTime GetProposalEndDate(int proposalKey);

        Automation.DataModels.Proposal GetProposal(int debtcounsellingkey, ProposalStatusEnum status, ProposalTypeEnum propType);

        Automation.DataModels.ProposalItem GetProposalItem(int proposalkey, DateTime startDate, DateTime endDate);

        QueryResults GetProposalDetails(int debtCounsellingKey, ProposalStatusEnum proposalStatus, ProposalAcceptedEnum proposalAccepted, ProposalTypeEnum proposalType);

        void InsertRecoveriesProposal(int accountKey, double shortfallAmt, double repaymentAmount, int AdUserKey, GeneralStatusEnum generalstatus);

        Automation.DataModels.RecoveriesProposal GetActiveRecoveriesProposalForAccount(int accountKey);

        QueryResults GetLatestProposalRecordByAccountKey(int accountKey, DebtCounsellingStatusEnum status);

        IEnumerable<Automation.DataModels.Proposal> GetProposals(params int[] debtCounsellingKey);

        IEnumerable<Automation.DataModels.ProposalItem> GetProposalItems(int proposalkey);

        void UpdateReviewDateOfActiveAcceptedProposal(int debtCounsellingKey);
    }
}