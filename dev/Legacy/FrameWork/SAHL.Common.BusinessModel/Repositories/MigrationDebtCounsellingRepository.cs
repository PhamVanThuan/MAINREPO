using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// Migration Debt Counselling Repository
    /// </summary>
    [FactoryType(typeof(IMigrationDebtCounsellingRepository))]
    public class MigrationDebtCounsellingRepository : AbstractRepositoryBase, IMigrationDebtCounsellingRepository
    {
        public MigrationDebtCounsellingRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public MigrationDebtCounsellingRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        /// <summary>
        /// Get by the Primary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMigrationDebtCounselling GetMigrationDebtCounsellingByKey(int key)
        {
            return base.GetByKey<IMigrationDebtCounselling, MigrationDebtCounselling_DAO>(key);
        }

        /// <summary>
        /// Get by the Account key
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IMigrationDebtCounselling GetMigrationDebtCounsellingByAccountKey(int accountKey)
        {
            MigrationDebtCounselling_DAO[] mdcDAO = MigrationDebtCounselling_DAO.FindAllByProperty("AccountKey", accountKey);

            if (mdcDAO != null && mdcDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IMigrationDebtCounselling>(mdcDAO[0]);
            }

            return null;
        }

        /// <summary>
        /// Create Empty Debt Counselling Case
        /// </summary>
        /// <returns></returns>
        public IMigrationDebtCounselling CreateEmptyDebtCounselling()
        {
            return base.CreateEmpty<IMigrationDebtCounselling, MigrationDebtCounselling_DAO>();
        }

        /// <summary>
        /// Save Debt Counselling Case
        /// </summary>
        /// <param name="debtCounselling"></param>
        public void SaveDebtCounselling(IMigrationDebtCounselling debtCounselling)
        {
            base.Save<IMigrationDebtCounselling, MigrationDebtCounselling_DAO>(debtCounselling);
        }

        /// <summary>
        /// Save Debt Counselling External Role
        /// </summary>
        /// <param name="dcER"></param>
        public void SaveMigrationDebtCounsellingExternalRole(IMigrationDebtCounsellingExternalRole dcER)
        {
            base.Save<IMigrationDebtCounsellingExternalRole, MigrationDebtCounsellingExternalRole_DAO>(dcER);
        }

        /// <summary>
        /// Delete Debt Counselling External Role
        /// </summary>
        /// <param name="dcER"></param>
        public void DeleteMigrationDebtCounsellingExternalRole(IMigrationDebtCounsellingExternalRole dcER)
        {
            //dcER
            MigrationDebtCounsellingExternalRole_DAO erDAO = MigrationDebtCounsellingExternalRole_DAO.Find(dcER.Key);

            erDAO.DeleteAndFlush();

            //base.<IMigrationDebtCounsellingExternalRole, MigrationDebtCounsellingExternalRole_DAO>(dcER);
        }

        /// <summary>
        /// Get Migration Proposal Item By Key
        /// </summary>
        /// <param name="proposalItemKey"></param>
        /// <returns></returns>
        public IMigrationDebtCounsellingProposalItem GetMigrationProposalItemByKey(int proposalItemKey)
        {
            return base.GetByKey<IMigrationDebtCounsellingProposalItem, MigrationDebtCounsellingProposalItem_DAO>(proposalItemKey);
        }

        /// <summary>
        /// Create Empty Proposal Item
        /// </summary>
        /// <returns></returns>
        public IMigrationDebtCounsellingProposalItem CreateEmptyProposalItem()
        {
            return base.CreateEmpty<IMigrationDebtCounsellingProposalItem, MigrationDebtCounsellingProposalItem_DAO>();
        }

        /// <summary>
        /// Create Empty Proposal
        /// </summary>
        /// <returns></returns>
        public IMigrationDebtCounsellingProposal CreateEmptyProposal()
        {
            return base.CreateEmpty<IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal_DAO>();
        }

        /// <summary>
        /// Get Migration Proposal By Key
        /// </summary>
        /// <param name="proposalKey"></param>
        /// <returns></returns>
        public IMigrationDebtCounsellingProposal GetMigrationProposalByKey(int proposalKey)
        {
            return base.GetByKey<IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal_DAO>(proposalKey);
        }

        /// <summary>
        /// Save Proposal
        /// </summary>
        /// <param name="proposal"></param>
        public void SaveMigrationProposal(IMigrationDebtCounsellingProposal proposal)
        {
            base.Save<IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal_DAO>(proposal);
        }

        /// <summary>
        /// Create Empty External Role
        /// </summary>
        /// <returns></returns>
        public IMigrationDebtCounsellingExternalRole CreateEmptyExternalRole()
        {
            return base.CreateEmpty<IMigrationDebtCounsellingExternalRole, MigrationDebtCounsellingExternalRole_DAO>();
        }

        /// <summary>
        /// get a list of all LE's on the account with an indication of whether the LE is inolved in the DC case
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public DataTable GetAccountLegalEntities(int AccountKey)
        {
            DataTable dt = new DataTable();

            string query = UIStatementRepository.GetStatement("Migrate", "GetAccountLegalEntities");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            // Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", AccountKey));

            // Execute Query
            DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
                dt = dsResults.Tables[0];

            return dt;
        }

        /// <summary>
        /// Get Approval Users
        /// </summary>
        /// <returns></returns>
        public DataTable GetApprovalUsers()
        {
            DataTable dt = new DataTable();

            string query = UIStatementRepository.GetStatement("Migrate", "GetApprovalUsers");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            // Execute Query
            DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
                dt = dsResults.Tables[0];

            return dt;
        }

        /// <summary>
        /// Get Consultant Users
        /// </summary>
        /// <returns></returns>
        public DataTable GetConsultantUsers()
        {
            DataTable dt = new DataTable();

            string query = UIStatementRepository.GetStatement("Migrate", "GetSAHLConsultants");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            // Execute Query
            DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
                dt = dsResults.Tables[0];

            return dt;
        }
    }
}