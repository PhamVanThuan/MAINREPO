using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IFinancialAdjustmentRepository))]
    public class FinancialAdjustmentRepository : AbstractRepositoryBase, IFinancialAdjustmentRepository
    {
        public FinancialAdjustmentRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public FinancialAdjustmentRepository()
        {
            this.castleTransactionService = new CastleTransactionsService();
        }

        private ICastleTransactionsService castleTransactionService;

        public IFinancialAdjustmentTypeSource GetFinancialAdjustmentTypeSourceByKey(int Key)
        {
            return base.GetByKey<IFinancialAdjustmentTypeSource, FinancialAdjustmentTypeSource_DAO>(Key);
        }

        public IEventList<IFinancialAdjustment> GetFinancialAdjustmentsByAccount(int AccountKey, int StageDefinitionStageDefinitionGroupKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.FinancialAdjustmentRepository", "GetFinancialAdjustmentsByAccount");
            SimpleQuery<FinancialAdjustment_DAO> query = new SimpleQuery<FinancialAdjustment_DAO>(QueryLanguage.Sql, sql, StageDefinitionStageDefinitionGroupKey, AccountKey);
            query.AddSqlReturnDefinition(typeof(FinancialAdjustment_DAO), "fa");
            FinancialAdjustment_DAO[] results = query.Execute();

            if (results != null && results.Length > 0)
            {
                return new DAOEventList<FinancialAdjustment_DAO, IFinancialAdjustment, FinancialAdjustment>(results);
            }

            return null;
        }

        public IFinancialAdjustment GetFinancialAdjustmentByKey(int financialAdjustmentKey)
        {
            string HQL = "from FinancialAdjustment_DAO fa where fa.Key = ?";

            SimpleQuery<FinancialAdjustment_DAO> query
                 = new SimpleQuery<FinancialAdjustment_DAO>(HQL, financialAdjustmentKey);

            query.SetQueryRange(1);
            FinancialAdjustment_DAO[] rateovDAO = query.Execute();

            if (rateovDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IFinancialAdjustment>(rateovDAO[0]);
            }
            return null;
        }

        public IFinancialAdjustment GetEmptyFinancialAdjustment()
        {
            return base.CreateEmpty<IFinancialAdjustment, FinancialAdjustment_DAO>();
        }

        public void SaveFinancialAdjustment(IFinancialAdjustment fadj)
        {
            base.Save<IFinancialAdjustment, FinancialAdjustment_DAO>(fadj);
        }

        #region Will be handled by the Back End API on Create or Cancel of a Financial Adjustment

        //public void RecalculateRate(int financialServiceKey, int rateConfigurationKey, string userID, string loanTransactionReference, bool baseRateReset)
        //{
        //    //const string query = "EXEC [Process].[halo].[pUpdateInterestRate] @RateConfigurationKey, @FinancialServiceKey, @UserID, @Msg";
        //    string query = UIStatementRepository.GetStatement("Repositories.FinancialAdjustmentRepository", "UpdateInterestRate");

        //    // Create a collection
        //    ParameterCollection prms = new ParameterCollection();
        //    //Add the required parameters
        //    prms.Add(new SqlParameter("@FinancialServiceKey", financialServiceKey));
        //    prms.Add(new SqlParameter("@RateConfigurationKey", rateConfigurationKey));

        //    SqlParameter userId = new SqlParameter("@UserId", userID);
        //    userId.Size = 50;
        //    userId.SqlDbType = SqlDbType.VarChar;
        //    prms.Add(userId);

        //    SqlParameter msg = new SqlParameter("@Msg",SqlDbType.VarChar);
        //    msg.Size = 1024;
        //    msg.Direction = ParameterDirection.Output;
        //    prms.Add(msg);
        //    // execute
        //    ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
        //}

        //public void UpdateInstalment(string userID, int financialServiceKey, string reference)
        //{
        //    string query = UIStatementRepository.GetStatement("Repositories.FinancialAdjustmentRepository", "UpdateInstalment");
        //    //const string query = "EXEC [Process].[halo].[pUpdateInstalment] @Msg , @FinancialServiceKey, @UserId, @Reference";

        //    // Create a collection
        //    ParameterCollection prms = new ParameterCollection();
        //    //Add the required parameters
        //    SqlParameter msg = new SqlParameter("@Msg", SqlDbType.VarChar);
        //    msg.Direction = ParameterDirection.Output;
        //    msg.Size = 1024;
        //    prms.Add(msg);
        //    prms.Add(new SqlParameter("@FinancialServiceKey", financialServiceKey));

        //    SqlParameter userId = new SqlParameter("@UserId", userID);
        //    userId.Size = 50;
        //    userId.SqlDbType = SqlDbType.VarChar;
        //    prms.Add(userId);

        //    prms.Add(new SqlParameter("@Reference", reference));

        //    // execute
        //    ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
        //}

        #endregion Will be handled by the Back End API on Create or Cancel of a Financial Adjustment

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialAdjustmentTypeSourceKey"></param>
        /// <returns></returns>
        public IFinancialAdjustmentTypeSource GetFinancialAdjustmentTypeSource(int financialAdjustmentTypeSourceKey)
        {
            return base.GetByKey<IFinancialAdjustmentTypeSource, FinancialAdjustmentTypeSource_DAO>(financialAdjustmentTypeSourceKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        public void DefendingDiscountOptOut(int accountKey, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.FinancialAdjustmentRepository", "DefendingDiscountOptOut", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialAdjustment"></param>
        /// <param name="userID"></param>
        public void DiscountedLinkRateOptIn(IFinancialAdjustment financialAdjustment, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", financialAdjustment.FinancialService.Key));
            prms.Add(new SqlParameter("@FinancialAdjustmentSourcekey", financialAdjustment.FinancialAdjustmentSource.Key));
            prms.Add(new SqlParameter("@Discount", financialAdjustment.InterestRateAdjustment.Adjustment));
            prms.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.FinancialAdjustmentRepository", "DiscountedLinkRateOptIn", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialAdjustment"></param>
        /// <param name="userID"></param>
        public void DiscountedLinkRateOptOut(IFinancialAdjustment financialAdjustment, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", financialAdjustment.FinancialService.Key));
            prms.Add(new SqlParameter("@FinancialAdjustmentSourcekey", financialAdjustment.FinancialAdjustmentSource.Key));
            prms.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.FinancialAdjustmentRepository", "DiscountedLinkRateOptOut", prms);
        }
    }
}