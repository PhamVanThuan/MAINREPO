using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class MortgageLoanRepositoryTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void GetReadvanceComparisonAmount()
        {
            using (new SessionScope())
            {
                string query = "select top 1 fs.AccountKey "
                    + "from [2am].[dbo].[FinancialService] fs (nolock) "
                    + "join [2am].[fin].[MortgageLoan] ml (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey "
                    + "join [2am].[dbo].[BondMortgageLoan] bml (nolock) on bml.FinancialServiceKey = ml.FinancialServiceKey "
                    + "join [2am].[dbo].[Bond] b (nolock) on b.BondKey = bml.BondKey "
                    + "where fs.AccountStatusKey in (1,5)";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int AccountKey = Convert.ToInt32(DT.Rows[0][0]);

                IMortgageLoanRepository repo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                double amount = repo.GetReadvanceComparisonAmount(AccountKey);
            }
        }

        [Test]
        public void InstallmentChange()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 a.AccountKey
                        from [2AM].[dbo].Account a with (nolock)
                        join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                        join [2AM].[fin].LoanBalance lb with (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
                        where a.AccountStatusKey = 1 and lb.RemainingInstalments > 170";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj == null)
                    Assert.Fail("No valid accounts for this test");

                int accKey = (int)obj;

                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

                try
                {
                    mortRepo.InstallmentChange(accKey, "BATCH", "Instalment Change");
                }
                catch (DomainValidationException dve)
                {
                    if (dve.Message.Contains("return code was: 999"))
                        Assert.Pass();
                }
            }
        }

        [Test]
        public void CreateEmptyMortgageLoan()
        {
            IMortgageLoanRepository repo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

            object ml = repo.CreateEmptyMortgageLoan();

            if (ml.GetType() == typeof(MortgageLoan))
            {
                Assert.IsTrue(ml.GetType() == typeof(MortgageLoan), "Mortgage Loan Created");
            }
            else
            {
                Assert.Fail("Unable to create Empty Mortgage Loan");
            }
        }

        [Test]
        public void TransferSPVTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 AccountKey from [2am].[dbo].Account where AccountStatusKey = 1  and ParentAccountKey is null order by AccountKey desc";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                int accKey = (int)obj;

                IMortgageLoanRepository repo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

                bool accIsOpen = true;
                try
                {
                    repo.TransferSPV(accKey, 28, "Unit Test");
                }
                catch (DomainValidationException dve)
                {
                    if (dve.Message.Contains("The Account is not open"))
                        accIsOpen = false;
                }

                Assert.True(accIsOpen);
            }
        }

        [Test]
        public void TransferSPVTestFail()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 AccountKey from [2am].[dbo].Account where AccountStatusKey = 2  and ParentAccountKey is null order by AccountKey desc";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                int accKey = (int)obj;

                IMortgageLoanRepository repo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();

                bool accIsOpen = false;
                try
                {
                    repo.TransferSPV(accKey, 28, "Unit Test");
                }
                catch (DomainValidationException dve)
                {
                    if (dve.Message.Contains("The Account is not open"))
                        accIsOpen = true;
                }

                Assert.True(accIsOpen);
            }
        }

        [Test]
        public void GetMortgageloanByAccountKey()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 a.AccountKey
                from [2AM].[dbo].Account a with (nolock)
                join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                join [2AM].[fin].LoanBalance lb with (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
                where a.AccountStatusKey = 1 and lb.RemainingInstalments > 170";

                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
                dt.Dispose();
                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IMortgageLoan ml = mortRepo.GetMortgageloanByAccountKey(accountKey);
                Assert.IsNotNull(ml);
                Assert.AreEqual(ml.Account.Key, accountKey);
            }
        }

        [NUnit.Framework.Test]
        public void GetMortgageloanByAccountKeyMock()
        {
            using (new SessionScope())
            {
                IMortgageLoan mlmock = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mlmock.Key).Return(1);
                IMortgageLoanRepository mlr = _mockery.StrictMock<IMortgageLoanRepository>();
                Expect.Call(mlr.GetMortgageloanByAccountKey(1)).Return(mlmock).IgnoreArguments();
                _mockery.ReplayAll();
                IMortgageLoan ml = mlr.GetMortgageloanByAccountKey(1);
                Assert.IsNotNull(ml);
            }
        }

        [Test]
        public void GetMortgageloanByKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 fs.FinancialServiceKey
                        from [2AM].[dbo].Account a with (nolock)
                        join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                        join [2AM].[fin].LoanBalance lb with (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
                        where a.AccountStatusKey = 1 and lb.RemainingInstalments > 170";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj == null)
                    Assert.Fail("No valid accounts for this test");

                int financialServiceKey = (int)obj;
                IMortgageLoanRepository repo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IMortgageLoan ml = repo.GetMortgageLoanByKey(financialServiceKey);

                Assert.IsNotNull(ml);
            }
        }

        [Test]
        public void GetMortgageLoanByKeyMock()
        {
            using (new SessionScope())
            {
                IMortgageLoan mlmock = _mockery.StrictMock<IMortgageLoan>();
                SetupResult.For(mlmock.Key).Return(1);
                IMortgageLoanRepository mlr = _mockery.StrictMock<IMortgageLoanRepository>();
                Expect.Call(mlr.GetMortgageLoanByKey(1)).Return(mlmock).IgnoreArguments();
                _mockery.ReplayAll();
                IMortgageLoan ml = mlr.GetMortgageLoanByKey(1);
                Assert.IsNotNull(ml);
            }
        }

        [Test]
        public void TermChange()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                string sql = @"select top 1 a.AccountKey
                    from [2AM].[dbo].Account a with (nolock)
                    join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                    join [2AM].[fin].LoanBalance lb with (nolock) on fs.FinancialServiceKey = lb.FinancialServiceKey
                    JOIN [2AM].spv.SPVMatrix spm ON a.SPVKey = spm.SPVKey
                    JOIN [2AM].spv.SPVRule sr ON spm.SPVRuleKey = sr.SPVRuleKey
                    where a.AccountStatusKey = 1 and lb.RemainingInstalments > 170";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj == null)
                    Assert.Fail("No valid accounts for this test");
                int accKey = (int)obj;

                try
                {
                    string message = string.Empty;
                    mortRepo.TermChange(accKey, 0, "test");
                    Assert.IsTrue(true, "Term Change Passed");
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void GetMortgageLoansByAccount()
        {
            // Test that the row count of the query matches the item count of the list
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 a.AccountKey
                            from [2AM].[dbo].Account a with (nolock)
                            join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                            join [2AM].[fin].MortgageLoan ml with (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
                            GROUP BY a.AccountKey having count(a.AccountKey) > 2";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
                dt.Dispose();
                sql = @"select a.AccountKey
                        from [2AM].[dbo].Account a with (nolock)
                        join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                        join [2AM].[fin].MortgageLoan ml with (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
                        where a.AccountKey = " + accountKey.ToString();
                dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int rowCount = Convert.ToInt32(dt.Rows.Count);
                dt.Dispose();
                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IReadOnlyEventList<IMortgageLoan> mlList = mortRepo.GetMortgageLoansByAccountKey(accountKey);

                Assert.IsNotNull(mlList);
                Assert.AreEqual(mlList.Count, rowCount);
            }
        }

        [Test]
        public void GetMortgageLoansByAccountEndWithFail()
        {
            // Test that confirms that IMortgageLoan does not contain Life Accounts
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 accountkey from offer with (nolock) where offertypekey = 5 and AccountKey is not null order by offerstartdate desc";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
                dt.Dispose();

                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IReadOnlyEventList<IMortgageLoan> mlList = mortRepo.GetMortgageLoansByAccountKey(accountKey);

                Assert.IsNotNull(mlList);

                Assert.IsFalse(mlList.Count > 0);
            }
        }

        [Test]
        public void GetMortgageLoansByAccountAndCheckIfSwitch()
        {
            // Test that the row count of the query matches the item count of the list
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 a.AccountKey
                            from [2AM].[dbo].Account a with (nolock)
                            join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                            join [2AM].[fin].MortgageLoan ml with (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
                            GROUP BY a.AccountKey , ml.mortgageloanpurposeKey having count(a.AccountKey) > 2 and ml.mortgageloanpurposeKey = 2";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
                dt.Dispose();
                sql = @"select a.AccountKey
                        from [2AM].[dbo].Account a with (nolock)
                        join [2AM].[dbo].FinancialService fs with (nolock) on fs.AccountKey = a.AccountKey
                        join [2AM].[fin].MortgageLoan ml with (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
                        where a.AccountKey = " + accountKey.ToString();
                dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                int rowCount = Convert.ToInt32(dt.Rows.Count);
                dt.Dispose();
                IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IReadOnlyEventList<IMortgageLoan> mlList = mortRepo.GetMortgageLoansByAccountKey(accountKey);

                foreach (IMortgageLoan ml in mlList)
                {
                    if (ml.MortgageLoanPurpose.Key == 2)
                    {
                        Assert.IsNotNull(mlList);
                        Assert.AreEqual(mlList.Count, rowCount);
                    }
                    else
                    {
                        Assert.Fail("Not a Switch Loan");
                    }
                }
            }
        }

        [Test]
        public void LookUpPendingTermChangeFromX2()
        {
            string sql = @"select top 1 InstanceID ,NewTerm  from x2.X2DATA.Loan_Adjustments order by InstanceID Desc";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int instanceID = Convert.ToInt32(dt.Rows[0]["InstanceID"]);
            int NewTerm = Convert.ToInt32(dt.Rows[0]["NewTerm"]);
            dt.Dispose();

            IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            int result = mortRepo.LookUpPendingTermChangeFromX2(instanceID);

            Assert.AreEqual(result, NewTerm);
        }

        [Test]
        public void LookUpPendingTermChangeByAccountShouldPass()
        {
            string sql = @"SELECT top 1   AccountKey
                            FROM  X2.X2DATA.Loan_Adjustments  LA with (nolock)
                            INNER JOIN  X2.X2.WorkList  WL with (nolock) ON LA.InstanceID = WL.InstanceID
                            WHERE (LA.RequestApproved = 0)
                            AND (LA.ProcessUser IS NULL)
                            AND WL.Message = 'Term Change Request' Order by LA.InstanceID desc";

            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
            dt.Dispose();

            IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            bool result = mortRepo.LookUpPendingTermChangeByAccount(accountKey);

            Assert.AreEqual(true, result, String.Format("LookUpPendingTermChangeByAccountShouldPass failed on accountKey: {0}", accountKey));
        }

        [Test]
        public void LookUpPendingTermChangeByAccountShouldFail()
        {
            string sql = @"SELECT top 1   AccountKey
                            FROM  X2.X2DATA.Loan_Adjustments  LA with (nolock)
                            INNER JOIN  X2.X2.WorkList  WL with (nolock) ON LA.InstanceID = WL.InstanceID
                            WHERE (LA.RequestApproved = 0)
                            AND (LA.ProcessUser IS NULL)
                            AND WL.Message = 'Term Request Timeout' Order by LA.InstanceID desc";

            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
            dt.Dispose();

            IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            bool result = mortRepo.LookUpPendingTermChangeByAccount(accountKey);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void GetNewSPVDescriptionMock()
        {
            using (new SessionScope())
            {
                string result = "test";
                IMortgageLoanRepository mlr = _mockery.StrictMock<IMortgageLoanRepository>();
                Expect.Call(mlr.GetNewSPVDescription(0)).Return(result).IgnoreArguments();
                _mockery.ReplayAll();
                mlr.GetNewSPVDescription(0);
            }
        }

        [Test]
        public void GetNewSPVDescription()
        {
            using (new SessionScope())
            {
                try
                {
                    IMortgageLoanRepository mlr = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                    mlr.GetNewSPVDescription(0);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void GetMortgageLoanPurposeByKeyMock()
        {
            using (new SessionScope())
            {
                IMortgageLoanPurpose mlmock = _mockery.StrictMock<IMortgageLoanPurpose>();
                SetupResult.For(mlmock.Key).Return(1);
                IMortgageLoanRepository mlr = _mockery.StrictMock<IMortgageLoanRepository>();
                Expect.Call(mlr.GetMortgageLoanPurposeByKey(1)).Return(mlmock).IgnoreArguments();
                _mockery.ReplayAll();
                IMortgageLoanPurpose ml = mlr.GetMortgageLoanPurposeByKey(1);
                Assert.IsNotNull(ml);
            }
        }

        [Test]
        public void GetMortgageLoanPurposeByKey()
        {
            IMortgageLoanRepository mortRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            IMortgageLoanPurpose mlp = mortRepo.GetMortgageLoanPurposeByKey(3);
            Assert.IsNotNull(mlp);
        }
    }
}