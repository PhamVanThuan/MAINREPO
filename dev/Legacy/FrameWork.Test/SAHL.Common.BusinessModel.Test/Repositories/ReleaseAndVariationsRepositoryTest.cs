using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ReleaseAndVariationsRepositoryTest : TestBase
    {
        [Test]
        public void GetBondByFinancialServiceKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                               "From [2am].[dbo].BondMortgageLoan BML (nolock) " +
                               "Inner Join [2am].[dbo].FinancialService FS (nolock) On BML.FinancialServiceKey = FS.FinancialServiceKey " +
                               "Where FS.AccountStatusKey = 1 ";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    List<IBond> bndList = rvRepo.GetBondByFinancialServiceKey(iAccountKey);
                    Assert.IsNotNull(bndList);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetHouseholdIncomeByAccountKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    double dHouseholdIncome = rvRepo.GetHouseholdIncomeByAccountKey(iAccountKey);
                    Assert.IsNotNull(dHouseholdIncome);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetAddressGivenAccountKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = String.Format(@"select top 1 fs.AccountKey
                            from fin.MortgageLoan ml
                            inner join dbo.Property p on ml.PropertyKey = p.PropertyKey
                            inner join dbo.FinancialService fs on ml.FinancialServiceKey=fs.FinancialServiceKey
                            , dbo.MortgageLoanPurpose mlp
                            where ML.MortgageLoanPurposeKey=mlp.MortgageLoanPurposeKey
                            and mlp.MortgageLoanPurposeGroupKey={0}
                            and fs.FinancialServiceTypeKey={1}",
                    Convert.ToInt32(Globals.MortgageLoanPurposeGroups.MortgageLoan).ToString(),
                    Convert.ToInt32(Globals.FinancialServiceTypes.VariableLoan).ToString()
                );

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    string strAddress = rvRepo.GetAddressGivenAccountKey(iAccountKey);
                    Assert.IsNotNull(strAddress);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetCurrentInstallment()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    double dCurrentInstallment = rvRepo.GetCurrentInstallment(iAccountKey);
                    Assert.IsNotNull(dCurrentInstallment);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetLatestValuationByPropertyKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 PropertyKey " +
                                  "From [2am].[dbo].Valuation (nolock) " +
                                  "Where IsActive = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iPropertyKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    double dLatestValuation = rvRepo.GetLatestValuationByPropertyKey(iPropertyKey);
                    Assert.IsNotNull(dLatestValuation);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void FindSPVNameByFinancialServicesKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    string strSPVName = rvRepo.FindSPVNameByFinancialServicesKey(iAccountKey);
                    Assert.IsNotNull(strSPVName);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetLoanCurrentBalance()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    double dCurrentBalance = rvRepo.GetLoanCurrentBalance(iAccountKey);
                    Assert.IsNotNull(dCurrentBalance);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetArrearBalanceByAccountKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    double dArrearBalance = rvRepo.GetArrearBalanceByAccountKey(iAccountKey);
                    Assert.IsNotNull(dArrearBalance);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetLegalEntities()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select top 1 FS.AccountKey " +
                                   "From [2am].[fin].MortgageLoan ML (nolock) " +
                                   "Inner Join [2am].[dbo].FinancialService FS (nolock) On  ML.FinancialServiceKey = FS.FinancialServiceKey " +
                                   "Where FS.AccountStatusKey = 1";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iAccountKey = Convert.ToInt32(obj);
                    IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                    IDomainMessageCollection messages = null;
                    string[] strLegalEntities = rvRepo.GetLegalEntities(iAccountKey, messages);
                    Assert.IsNotNull(strLegalEntities);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void CreateReleaseAndVariationsDataSet()
        {
            IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
            DataSet dsRV = rvRepo.CreateReleaseAndVariationsDataSet();
            Assert.IsNotNull(dsRV);
        }

        [Test]
        public void CreateConditionsTable()
        {
            IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
            DataTable dtConditions = rvRepo.CreateConditionsTable();
            Assert.IsNotNull(dtConditions);
        }

        [Test]
        public void CreateBondDetailsTable()
        {
            IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
            DataTable dtBondDetails = rvRepo.CreateBondDetailsTable();
            Assert.IsNotNull(dtBondDetails);
        }

        [Test]
        public void CreateRequestTypesTable()
        {
            IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
            DataTable dtRequestTypes = rvRepo.CreateRequestTypesTable();
            Assert.IsNotNull(dtRequestTypes);
        }

        [Test]
        public void CreateGetChangeTypesTable()
        {
            IReleaseAndVariationsRepository rvRepo = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
            DataTable dtChangeTypes = rvRepo.CreateGetChangeTypesTable();
            Assert.IsNotNull(dtChangeTypes);
        }
    }
}