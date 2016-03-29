using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Test;
using System;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class FinancialAdjustmentRepositoryTest : TestBase
    {
        [Test]
        public void GetEmptyFinancialAdjustment()
        {
            IFinancialAdjustmentRepository faRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
            IFinancialAdjustment financialAdjustment = faRepo.GetEmptyFinancialAdjustment();
            Assert.IsNotNull(financialAdjustment);
        }

        [Test]
        public void GetFinancialAdjustmentByKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "select top 1 FinancialAdjustmentKey " +
                                  "from [fin].FinancialAdjustment fa (nolock) " +
                                  "join [fin].FinancialAdjustmentTypeSource fats (nolock) on fats.FinancialAdjustmentSourceKey = fa.FinancialAdjustmentSourceKey and fats.FinancialAdjustmentTypeKey = fa.FinancialAdjustmentTypeKey " +
                                  "where fats.FinancialAdjustmentTypeSourceKey = 4 or FinancialAdjustmentTypeSourceKey = 8";
                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                if (obj != null)
                {
                    int financialAdjustmentKey = Convert.ToInt32(obj);
                    IFinancialAdjustmentRepository faRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();

                    IFinancialAdjustment financialAdjustment = faRepo.GetFinancialAdjustmentByKey(financialAdjustmentKey);
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        [Test]
        public void DefendingDiscountOptOutPass()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string sqlQuery = @"select top 1
											fs.AccountKey, fa.*
										from
												[2am].dbo.Account acc
										join	[2am].dbo.FinancialService fs on acc.AccountKey = fs.AccountKey and RRR_ProductKey = 6
										join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
										and fa.FinancialAdjustmentSourceKey = 9
										and fa.FinancialAdjustmentTypeKey = 2
										and fa.FinancialAdjustmentStatusKey = 1
										and fs.ParentFinancialServiceKey is null
										and fs.AccountStatusKey = 1
										and fa.FromDate < getdate()
										";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int accountKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        financialAdjustmentRepository.DefendingDiscountOptOut(accountKey, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }

        [Test]
        public void DefendingDiscountOptOutFail()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string sqlQuery = @"select top 1
										fs.AccountKey, fa.*
									from
											[2am].dbo.FinancialService fs
									join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
									and fa.FinancialAdjustmentSourceKey = 9
									and fa.FinancialAdjustmentTypeKey = 2
									and fs.ParentFinancialServiceKey is null
									and fs.AccountStatusKey = 2";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int accountKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        financialAdjustmentRepository.DefendingDiscountOptOut(accountKey, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                catch (DomainValidationException)
                {
                    Assert.Pass("Pass");
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }

        [Test]
        public void DiscountedLinkRateOptInPass()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string sqlQuery = @"select top 1
											fa.FinancialAdjustmentKey
										from
												[2am].dbo.FinancialService fs
										join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
										and fa.FinancialAdjustmentSourceKey = 10
										and fa.FinancialAdjustmentTypeKey = 2
										and fs.ParentFinancialServiceKey is null
										and fs.AccountStatusKey = 1
										and fa.FinancialAdjustmentStatusKey = 1";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int financialAdjustmentKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        var financialAdjustment = financialAdjustmentRepository.GetFinancialAdjustmentByKey(financialAdjustmentKey);
                        financialAdjustmentRepository.DiscountedLinkRateOptIn(financialAdjustment, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }

        [Test]
        public void DiscountedLinkRateOptInFail()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string sqlQuery = @"select top 1
											fa.FinancialAdjustmentKey
										from
												[2am].dbo.FinancialService fs
										join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
										and fa.FinancialAdjustmentSourceKey = 10
										and fa.FinancialAdjustmentTypeKey = 2
										and fs.ParentFinancialServiceKey is null
										and fs.AccountStatusKey = 1
										and fa.FinancialAdjustmentStatusKey = 1";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int financialAdjustmentKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        var financialAdjustment = financialAdjustmentRepository.GetFinancialAdjustmentByKey(financialAdjustmentKey);
                        financialAdjustmentRepository.DiscountedLinkRateOptIn(financialAdjustment, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                catch (DomainValidationException)
                {
                    Assert.Pass("Pass");
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }

        [Test]
        public void DiscountedLinkRateOptOutPass()
        {
            using (var transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    string sqlQuery = @"select top 1
											fa.FinancialAdjustmentKey
										from
												[2am].dbo.FinancialService fs
										join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
										and fa.FinancialAdjustmentSourceKey = 10
										and fa.FinancialAdjustmentTypeKey = 2
										and fs.ParentFinancialServiceKey is null
										and fs.AccountStatusKey = 1
										and fa.FinancialAdjustmentStatusKey = 1     ";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int financialAdjustmentKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        var financialAdjustment = financialAdjustmentRepository.GetFinancialAdjustmentByKey(financialAdjustmentKey);
                        financialAdjustmentRepository.DiscountedLinkRateOptOut(financialAdjustment, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }

        [Test]
        public void DiscountedLinkRateOptOutFail()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string sqlQuery = @"select top 1
											fa.FinancialAdjustmentKey
										from
												[2am].dbo.FinancialService fs
										join	[2am].fin.FinancialAdjustment fa on fs.FinancialServiceKey = fa.FinancialServiceKey
										and fa.FinancialAdjustmentSourceKey = 5
										and fs.ParentFinancialServiceKey is null
										and fs.AccountStatusKey = 1
										and fa.FinancialAdjustmentStatusKey = 3";
                    ParameterCollection parameters = new ParameterCollection();

                    object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);

                    if (obj != null)
                    {
                        int financialAdjustmentKey = Convert.ToInt32(obj);
                        var financialAdjustmentRepository = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                        var financialAdjustment = financialAdjustmentRepository.GetFinancialAdjustmentByKey(financialAdjustmentKey);
                        financialAdjustmentRepository.DiscountedLinkRateOptOut(financialAdjustment, "System");
                    }
                    else
                    {
                        Assert.Fail("No valid keys for this test");
                    }
                }
                catch (DomainValidationException ex)
                {
                    Assert.That(ex.Message.Contains("This Financialservicekey is not on Discounted Link Rate"));
                }
                finally
                {
                    transactionScope.VoteRollBack();
                }
            }
        }
    }
}