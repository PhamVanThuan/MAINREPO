using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// Migration Debt Counselling Repository Contract
    /// </summary>
    public interface IMigrationDebtCounsellingRepository
    {
        /// <summary>
        /// Get by the Primary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IMigrationDebtCounselling GetMigrationDebtCounsellingByKey(int key);

        /// <summary>
        /// Get by the Account key
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IMigrationDebtCounselling GetMigrationDebtCounsellingByAccountKey(int accountKey);

        /// <summary>
        /// Create Empty Debt Counselling Case
        /// </summary>
        /// <returns></returns>
        IMigrationDebtCounselling CreateEmptyDebtCounselling();

        /// <summary>
        /// Save Debt Counselling Case
        /// </summary>
        /// <param name="debtCounselling"></param>
        void SaveDebtCounselling(IMigrationDebtCounselling debtCounselling);

        /// <summary>
        /// Save Debt Counselling External Role
        /// </summary>
        /// <param name="dcER"></param>
        void SaveMigrationDebtCounsellingExternalRole(IMigrationDebtCounsellingExternalRole dcER);

        /// <summary>
        /// Delete Debt Counselling External Role
        /// </summary>
        /// <param name="dcER"></param>
        void DeleteMigrationDebtCounsellingExternalRole(IMigrationDebtCounsellingExternalRole dcER);

        /// <summary>
        /// Get Migration Proposal Item By Key
        /// </summary>
        /// <param name="proposalItemKey"></param>
        /// <returns></returns>
        IMigrationDebtCounsellingProposalItem GetMigrationProposalItemByKey(int proposalItemKey);

        /// <summary>
        /// Create Empty Proposal Item
        /// </summary>
        /// <returns></returns>
        IMigrationDebtCounsellingProposalItem CreateEmptyProposalItem();

        /// <summary>
        /// Create Empty Proposal
        /// </summary>
        /// <returns></returns>
        IMigrationDebtCounsellingProposal CreateEmptyProposal();

        /// <summary>
        /// Get Migration Proposal By Key
        /// </summary>
        /// <param name="proposalKey"></param>
        /// <returns></returns>
        IMigrationDebtCounsellingProposal GetMigrationProposalByKey(int proposalKey);

        /// <summary>
        /// Save Proposal
        /// </summary>
        /// <param name="proposal"></param>
        void SaveMigrationProposal(IMigrationDebtCounsellingProposal proposal);

        /// <summary>
        /// Create Empty External Role
        /// </summary>
        /// <returns></returns>
        IMigrationDebtCounsellingExternalRole CreateEmptyExternalRole();

        /// <summary>
        /// get a list of all LE's on the account with an indication of whether the LE is inolved in the DC case
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        DataTable GetAccountLegalEntities(int AccountKey);

        /// <summary>
        /// Get Approval Users
        /// </summary>
        /// <returns></returns>
        DataTable GetApprovalUsers();

        /// <summary>
        /// Get Consultant Users
        /// </summary>
        /// <returns></returns>
        DataTable GetConsultantUsers();
    }
}