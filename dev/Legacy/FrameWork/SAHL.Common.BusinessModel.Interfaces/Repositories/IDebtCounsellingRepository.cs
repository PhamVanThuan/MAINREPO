using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataSets;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IDebtCounsellingRepository
    {
        /// <summary>f
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDebtCounselling GetDebtCounsellingByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="debtCounsellingStatus"></param>
        /// <returns></returns>
        List<IDebtCounselling> GetDebtCounsellingByAccountKey(int accountKey, DebtCounsellingStatuses debtCounsellingStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        List<IDebtCounselling> GetDebtCounsellingByAccountKey(int accountKey);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="accountKey"></param>
        ///// <param name="activeOnly"></param>
        ///// <returns></returns>
        //List<IDebtCounselling> GetDebtCounsellingByAccountKey(int accountKey, bool activeOnly);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IDebtCounselling CreateEmptyDebtCounselling();

        /// <summary>
        /// Create Empty Debt Counselling Group
        /// </summary>
        /// <returns></returns>
        IDebtCounsellingGroup CreateEmptyDebtCounsellingGroup();

        /// <summary>
        /// Get Debt Counselling Group by Key
        /// </summary>
        /// <param name="debtCounsellingGroupKey"></param>
        /// <returns></returns>
        IDebtCounsellingGroup GetDebtCounsellingGroupByKey(int debtCounsellingGroupKey);

        /// <summary>
        /// Save Debt Counselling Group
        /// </summary>
        /// <param name="debtCounsellingGroup"></param>
        void SaveDebtCounsellingGroup(IDebtCounsellingGroup debtCounsellingGroup);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounselling"></param>
        void SaveDebtCounselling(IDebtCounselling debtCounselling);

        /// <summary>
        /// Get Debt Counselling Snap Shot
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        ISnapShotAccount GetDebtCounsellingSnapShot(int debtCounsellingKey);

        /// <summary>
        /// Get Post Debt Counselling Mortgage Loan Installment
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        void GetPostDebtCounsellingMortgageLoanInstallment(int debtCounsellingKey, out double preDCInstalment, out double linkRate, out double marketRate, out double interestRate, out int term);

        /// <summary>
        /// The method does base calculations before calculating the amortising points.
        /// Gets the Loan Current Balance at a point in time.
        /// Calculates the Months Elapsed to determine the starting point.
        /// Calculates the Remaining Term.
        /// Calculates the Instalment.
        /// </summary>
        /// <param name="ProposalKey"></param>
        /// <returns></returns>
        LoanCalculations.AmortisationScheduleDataTable GetAmortisationScheduleForAccountByProposalKey(int ProposalKey);

        /// <summary>
        /// The method does base calculations before calculating the amortising points per proposal line item.
        /// Gets the Loan Current Balance at a point in time.
        /// Calculates the Months Elapsed to determine the starting point.
        /// </summary>
        /// <param name="ProposalKey"></param>
        /// <param name="maxPeriods"></param>
        /// <returns></returns>
        LoanCalculations.AmortisationScheduleDataTable GetAmortisationScheduleForProposalByKey(int ProposalKey, int maxPeriods);

        //LoanCalculations.AmortisationScheduleDataTable AgregateAmortisingPointsByInterval(LoanCalculations.AmortisationScheduleDataTable AmortisingTable, Intervals Interval);

        //LoanCalculations.AmortisationScheduleDataTable GetAmortisationSchedule(double LoanBalance, double Instalment, double InterestRate);

        //LoanCalculations.AmortisationScheduleDataTable GetAmortisationSchedule(double LoanBalance, double Instalment, double InterestRate, int StartingPeriod);

        /// <summary>
        /// Cancels a Debit counselling Case
        /// </summary>
        /// <param name="debtCounselling"></param>
        /// <param name="userID"></param>
        /// <param name="debtCounsellingStatus"></param>
        /// <returns></returns>
        void CancelDebtCounselling(IDebtCounselling debtCounselling, string userID, DebtCounsellingStatuses debtCounsellingStatus);

        /// <summary>
        /// Setup DC rates and overrides
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        void ConvertDebtCounselling(int accountKey, string userID);

        /// <summary>
        /// Opt into New Variable so that DC can be applied
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        void ProcessDebtCounsellingOptOut(int accountKey, string user);

        /// <summary>
        ///
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        IList<IDebtCounsellingGroup> GetRelatedDebtCounsellingGroupForLegalEntities(List<int> keys);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UpdateDebtCounsellingDebtReviewArrangement(int accountKey, string userName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool RollbackTransaction(int debtCounsellingKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        void CreateAccountSnapShot(int debtCounsellingKey);

        /// <summary>
        /// Gets the last active user of a given workflow role type which have been allocated to the case
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="workflowRoleType"></param>
        /// <returns>an IADuser or null</returns>
        IADUser GetActiveDebtCounsellingUser(int debtCounsellingKey, WorkflowRoleTypes workflowRoleType);

        /// <summary>
        /// Get the 60 days date
        /// This is x2 data so not exposed on the business model
        /// otherwise this will get run every time we load a DC case
        /// </summary>
        /// <param name="dcKey"></param>
        /// <returns></returns>
        DateTime? Get60DaysDate(int dcKey);

        int GetRemainingTermPriorToProposalAcceptance(int debtCounsellingKey);

        #region Attorney

        /// <summary>
        /// Get Litigation Attorneys
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetLitigationAttorneys();

        ///// <summary>
        ///// Get Debt Counselling Cases for Attorney
        ///// </summary>
        ///// <param name="legalEntityKey"></param>
        ///// <param name="attorneyStatus"></param>
        ///// <returns></returns>
        //List<IDebtCounselling> GetDebtCounsellingCasesForAttorney(int legalEntityKey, GeneralStatuses attorneyStatus);

        #endregion Attorney

        #region Loss Control

        /// <summary>
        ///
        /// </summary>
        /// <param name="eFolderID"></param>
        /// <returns></returns>
        IADUser GetEworkADUserForLossControlCase(string eFolderID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="eStageName"></param>
        /// <param name="eFolderID"></param>
        /// <param name="adUser"></param>
        void GetEworkDataForLossControlCase(int AccountKey, out string eStageName, out string eFolderID, out IADUser adUser);

        #endregion Loss Control

        #region Proposals

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IProposal CreateEmptyProposal();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IProposalItem CreateEmptyProposalItem();

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        void SaveProposal(IProposal proposal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposalKey"></param>
        /// <returns></returns>
        IProposal GetProposalByKey(int proposalKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposalItemKey"></param>
        /// <returns></returns>
        IProposalItem GetProposalItemByKey(int proposalItemKey);

        /// <summary>
        /// Return a List of Debt Counselling proposals by generickey and generickeytype
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <returns></returns>
        List<IProposal> GetProposalsByGenericKey(int genericKey, int genericKeyTypeKey);

        /// <summary>
        /// Get the Proposal Detail Items by Key
        /// </summary>
        /// <param name="ProposalKey"></param>
        /// <returns></returns>
        List<IProposalItem> GetProposalItemsByKey(int ProposalKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="proposalType"></param>
        /// <returns></returns>
        List<IProposal> GetProposalsByType(int debtCounsellingKey, SAHL.Common.Globals.ProposalTypes proposalType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="proposalStatus"></param>
        /// <returns></returns>
        List<IProposal> GetProposalsByStatus(int debtCounsellingKey, SAHL.Common.Globals.ProposalStatuses proposalStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="proposalType"></param>
        /// <param name="proposalStatus"></param>
        /// <returns></returns>
        List<IProposal> GetProposalsByTypeAndStatus(int debtCounsellingKey, SAHL.Common.Globals.ProposalTypes proposalType, SAHL.Common.Globals.ProposalStatuses proposalStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        /// <param name="adUser"></param>
        void CopyProposalToDraft(IProposal proposal, IADUser adUser);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        /// <param name="adUser"></param>
        /// <param name="proposalTypes"></param>
        void CopyProposalToDraft(IProposal proposal, IADUser adUser, ProposalTypes proposalTypes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        void DeleteProposal(IProposal proposal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposal"></param>
        /// <param name="adUser"></param>
        void SetProposalToActive(IProposal proposal, IADUser adUser);

        /// <summary>
        /// Sort Proposal Items
        /// </summary>
        /// <param name="proposal"></param>
        /// <returns></returns>
        IEventList<IProposalItem> SortProposalItems(IProposal proposal);

        #endregion Proposals

        #region DebtCounsellor

        /// <summary>
        ///
        /// </summary>
        /// <param name="eaos"></param>
        void SaveDebtCounsellorOrganisationStructure(IDebtCounsellorOrganisationNode eaos);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode GetDebtCounsellorOrganisationNodeForKey(int Key);

        /// <summary>
        /// Get the IPaymentDistributionAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode GetDebtCounsellorOrganisationNodeForLegalEntity(int key);

        /// <summary>
        /// Get an empty instance of debt counselling to work with
        /// </summary>
        /// <returns></returns>
        IDebtCounsellorDetail CreateEmptyDebtCounsellorDetail();

        /// <summary>
        /// Save a debt counsellor instance
        /// </summary>
        /// <param name="debtCounsellorDetail"></param>
        void SaveDebtCounsellorDetail(IDebtCounsellorDetail debtCounsellorDetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCouncellingKey"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode GetTopDebtCounsellorCompanyNodeForDebtCounselling(int debtCouncellingKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCouncellingKey"></param>
        /// <returns></returns>
        ILegalEntity GetDebtCounsellorForDebtCounselling(int debtCouncellingKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellorKey"></param>
        /// <returns></returns>
        ILegalEntity GetDebtCounsellorCompanyForDebtCounsellor(int debtCounsellorKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCouncellingKey"></param>
        /// <returns></returns>
        IEventList<ILegalEntity> GetAllDebtCounsellorsForDebtCounselling(int debtCouncellingKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="iDebtCounsellingKey"></param>
        /// <returns></returns>
        DataTable GetRelatedDebtCounsellingAccounts(int iDebtCounsellingKey);

        IList<IDebtCounselling> GetDebtCounsellingByLegalEntityKey(GenericKeyTypes genericKeyType, List<int> externalRoleTypeKeys, GeneralStatuses externalRoleGeneralStatus,
            List<int> roleTypeKeys, GeneralStatuses roleGeneralStatus, DebtCounsellingStatuses debtCounsellingStatus, int legalEntityKey);

        #endregion DebtCounsellor

        #region Payment Distribution Agent

        /// <summary>
        ///
        /// </summary>
        /// <param name="eaos"></param>
        void SavePaymentDistributionAgentOrganisationStructure(IPaymentDistributionAgentOrganisationNode eaos);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForKey(int Key);

        /// <summary>
        /// Get the IPaymentDistributionAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForLegalEntity(int key);

        #endregion Payment Distribution Agent

        #region Court / Hearing Detail

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICourt GetCourtByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICourtType GetCourtTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IHearingType GetHearingTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IHearingDetail CreateEmptyHearingDetail();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IHearingAppearanceType GetHearingAppearanceTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="HearingTypeKey"></param>
        /// <param name="HearingAppearanceTypeKeys"></param>
        /// <returns></returns>
        bool CheckHearingDetailExistsForDebtCounsellingKey(int debtCounsellingKey, int HearingTypeKey, List<int> HearingAppearanceTypeKeys);

        /// <summary>
        /// Get by the Primary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IHearingDetail GetHearingDetailByKey(int key);

        /// <summary>
        /// Save a hearing detail instance
        /// </summary>
        /// <param name="hearingDetail"></param>
        void SaveHearingDetail(IHearingDetail hearingDetail);

        #endregion Court / Hearing Detail

        #region Loan Details Types

        /// <summary>
        /// If a specific detail type is a added then raise an External Activity that willl cause a flag to fire in
        /// the Debt Counselling Workflow Map
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="detailTypeKey"></param>
        void RaiseActiveExternalActivityForAddDetailType(int accountKey, int detailTypeKey);

        /// <summary>
        /// If a specific detail type is a removed then raise an External Activity that willl cause a flag to fire in
        /// the Debt Counselling Workflow Map
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="detailTypeKey"></param>
        void RaiseActiveExternalActivityForDeleteDetailType(int accountKey, int detailTypeKey);

        #endregion Loan Details Types

        void SendNotification(IDebtCounselling debtCounselling);

        int GetExternalRoleTypeKeyForDebtCounsellingKeyAndLegalEntityKey(int debtCounsellingKey, int legalEntityKey);

        List<IDebtCounselling> SearchDebtCounsellingCasesForAttorney(int legalEntityKey, string IDNumber, int? accountKey, string legalEntityName);
    }
}