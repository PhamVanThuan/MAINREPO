using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using System.Data.SqlClient;
using SAHL.Common.DataAccess;
using System.Data;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IRateOverrideRepository))]
    public class RateOverrideRepository : AbstractRepositoryBase, IRateOverrideRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="StageDefinitionStageDefinitionGroupKey"></param>
        /// <returns></returns>
        public DataTable GetRateOverridesByAccount(int AccountKey, int StageDefinitionStageDefinitionGroupKey)
        {
            string sql = UIStatementRepository.GetStatement("COMMON", "GetRateOverridesByAccount");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", AccountKey));
            parameters.Add(new SqlParameter("@StageDefinitionStageDefinitionGroupKey", StageDefinitionStageDefinitionGroupKey));

            DataTable dtRateOverrides = new DataTable();
            DataSet dsRateOverrides = new DataSet();
            dsRateOverrides = ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
            if (dsRateOverrides != null)
            {
                if (dsRateOverrides.Tables.Count > 0)
                {
                    dtRateOverrides = dsRateOverrides.Tables[0];
                }
            }
            return dtRateOverrides;
        }



        public IRateOverride GetRateOverrideByKey(int RateOverrideKey)
        {
            string HQL = "from RateOverride_DAO rateov where rateov.Key = ?";

            SimpleQuery<RateOverride_DAO> query
                 = new SimpleQuery<RateOverride_DAO>(HQL, RateOverrideKey);

            query.SetQueryRange(1);
            RateOverride_DAO[] rateovDAO = query.Execute();

            if (rateovDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IRateOverride>(rateovDAO[0]);
            }
            return null;
        }

        public IRateOverride GetEmptyRateOverride()
        {
            return base.CreateEmpty<IRateOverride, RateOverride_DAO>();

        }

        public void SaveRateOverride(IRateOverride RateOverride)
        {
            base.Save<IRateOverride, RateOverride_DAO>(RateOverride);

        }

        public void RecalculateRate(int financialServiceKey, int rateConfigurationKey, string userID, string loanTransactionReference, bool baseRateReset)
        {
            const string query = "EXEC [2AM].[dbo].[pLoanUpdateRate] @FinancialServiceKey, @RateConfigurationKey, @UserId, @LoanTransactionReference, @BaseRateReset";

            // Create a collection
            ParameterCollection prms = new ParameterCollection();
            //Add the required parameters
            prms.Add(new SqlParameter("@FinancialServiceKey", financialServiceKey));
            prms.Add(new SqlParameter("@RateConfigurationKey", rateConfigurationKey));
            prms.Add(new SqlParameter("@UserId", userID));

            if (loanTransactionReference == null)
                prms.Add(new SqlParameter("@LoanTransactionReference", DBNull.Value));
            else
                prms.Add(new SqlParameter("@LoanTransactionReference", loanTransactionReference));

            prms.Add(new SqlParameter("@BaseRateReset", baseRateReset));
            // execute
            ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
        }

        public void UpdateInstalment(string userID, int AccountKey)
        {
            const string query = "EXEC [2AM].[dbo].[pLoanUpdateInstalment] @AccountKey, @UserId";

            // Create a collection
            ParameterCollection prms = new ParameterCollection();
            //Add the required parameters
            prms.Add(new SqlParameter("@AccountKey", AccountKey));
            prms.Add(new SqlParameter("@UserId", userID));

            // execute
            ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
        }


        public int ConvertDefendingDiscount(int LoanNumber, ref string ErrorDescription)
        {
            const string query = "EXEC [sahldb].[dbo].[ff_ConvertDefendingDiscount] @LoanNumber, @ErrorDescription output";

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LoanNumber", LoanNumber));
            prms.Add(new SqlParameter("@ErrorDescription", SqlDbType.VarChar)); // optional
            prms[1].Direction = ParameterDirection.Output;
            prms[1].Size = 256;


            object obj = ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
            int result = Convert.ToInt32(obj);

            ErrorDescription = prms[1].Value.ToString();

            return result;
        }
    }
}
