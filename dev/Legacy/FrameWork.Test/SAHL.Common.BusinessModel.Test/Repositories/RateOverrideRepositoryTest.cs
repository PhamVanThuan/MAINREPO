using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class RateOverrideRepositoryTest : TestBase
    {
        [Test]
        public void GetEmptyRateOverride()
        {
            IRateOverrideRepository roRepo = RepositoryFactory.GetRepository<IRateOverrideRepository>();
            IRateOverride rateOverride = roRepo.GetEmptyRateOverride();
            Assert.IsNotNull(rateOverride);
        }

        [Test]
        public void GetRateOverrideByKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select Top 1 RateOverrideKey " +
                                  "From RateOverride " +
                                  "Where RateOverrideTypeKey = 4 or RateOverrideTypeKey = 8";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CommonRepository.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int iRateOverrideKey = Convert.ToInt32(obj);
                    IRateOverrideRepository roRepo = RepositoryFactory.GetRepository<IRateOverrideRepository>();

                    IRateOverride rateOverride = roRepo.GetRateOverrideByKey(iRateOverrideKey);

                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }

            }
        }

        [Test]
        public void UpdateInstallment()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 A.AccountKey" +
                                  " From [2AM].[DBO].[Account] A (nolock)" +
                                  " Inner Join [2AM].[DBO].[FinancialService] FS (nolock) On FS.AccountKey = A.AccountKey" +
                                  " Inner Join [2AM].[DBO].[RateOverride] RO (nolock) On FS.FinancialServiceKey = RO.FinancialServiceKey";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CommonRepository.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IRateOverrideRepository roRepo = RepositoryFactory.GetRepository<IRateOverrideRepository>();

                    roRepo.UpdateInstalment("SAHL\\BCUser", iAccountKey);

                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }

            }
        }

        [Test]
        public void ConvertDefendingDiscount()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 A.AccountKey" +
                                  " From [2AM].[DBO].[Account] A (nolock)" +
                                  " Inner Join [2AM].[DBO].[FinancialService] FS (nolock) On FS.AccountKey = A.AccountKey" +
                                  " Inner Join [2AM].[DBO].[RateOverride] RO (nolock) On FS.FinancialServiceKey = RO.FinancialServiceKey";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CommonRepository.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IRateOverrideRepository roRepo = RepositoryFactory.GetRepository<IRateOverrideRepository>();

                    string ErrorDescription = "";
                    roRepo.ConvertDefendingDiscount(iAccountKey, ref ErrorDescription);

                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }

            }
        }

        [Test]
        public void RecalculateRate()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 A.AccountKey, FS.FinancialServiceKey, ML.RateConfigurationKey " +
                                  "From [2AM].[DBO].[Account] A (nolock) " +
	                              "Inner Join [2AM].[DBO].[FinancialService] FS (nolock) On FS.AccountKey = A.AccountKey " +
	                              "Inner Join [2AM].[DBO].[RateOverride] RO (nolock) On FS.FinancialServiceKey = RO.FinancialServiceKey " +
	                              "Inner Join [2AM].[DBO].[MortgageLoan] ML (nolock) On ML.FinancialServiceKey = FS.FinancialServiceKey";

                ParameterCollection parameters = new ParameterCollection();

                DataSet dsAccount = CommonRepository.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (dsAccount != null)
                {
                    int iFinancialServiceKey = Convert.ToInt32(dsAccount.Tables[0].Rows[0]["FinancialServiceKey"]);
                    int iRateConfigurationKey = Convert.ToInt32(dsAccount.Tables[0].Rows[0]["RateConfigurationKey"]);
                    IRateOverrideRepository roRepo = RepositoryFactory.GetRepository<IRateOverrideRepository>();

                    roRepo.RecalculateRate(iFinancialServiceKey, iRateConfigurationKey, "SAHL\\BCUser", null, false);

                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }

            }
        }
    }
}
