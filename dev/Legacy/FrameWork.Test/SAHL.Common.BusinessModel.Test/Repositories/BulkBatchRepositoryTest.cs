using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel;


namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class BulkBatchRepositoryTest : TestBase
    {
        [Test]
        public void GetBatchLoanTransactions()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 ft.FinancialTransactionKey from [2am].[fin].[FinancialTransaction] ft (NOLOCK)
								join [2am].[dbo].FinancialService fs on fs.FinancialServiceKey = ft.FinancialServiceKey
								where ft.Reference is not null";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int ltKey = Convert.ToInt32(DT.Rows[0][0]);

                FinancialTransaction_DAO lt = FinancialTransaction_DAO.Find(ltKey);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IReadOnlyEventList<IFinancialTransaction> list = repo.GetBatchLoanTransactions(lt.FinancialService.Account.Key, lt.TransactionType.Key, lt.EffectiveDate, lt.Reference, lt.UserID);

            }
        }

        [Test]
        public void GetBatchTransactionByKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 BatchTransactionKey from BatchTransaction";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);
                //BatchTransaction_DAO bt = BatchTransaction_DAO.Find(key);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IBatchTransaction ibt = repo.GetBatchTransactionByKey(key);
                Assert.IsNotNull(ibt);
            }
        }

        [Test]
        public void GetBulkBatchTransactionsByBulkBatchTypeKey()
        {
            using (new SessionScope())
            {
                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IList<IBulkBatch> list = repo.GetBulkBatchTransactionsByBulkBatchTypeKey(1);

                repo.SaveBulkBatch(list[0]);

                Assert.That(list.Count > 0);
            }
        }

        [Test]
        public void GetBulkBatchByBulkBatchTypeAndStatusKey()
        {
            using (new SessionScope())
            {
                string query = @"select count(*) from BulkBatch where BulkBatchTypeKey = 1 and BulkBatchStatusKey = 2";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int count = Convert.ToInt32(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IList<IBulkBatch> list = repo.GetBulkBatchByBulkBatchTypeAndStatusKey(1, 2);
                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetBatchLogByBatchKeyMessageType()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 BulkBatchKey, MessageTypeKey, count(BulkBatchLogKey) from BulkBatchLog 
								group by BulkBatchKey, MessageTypeKey";

                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);
                int mt = Convert.ToInt32(DT.Rows[0][1]);
                int count = Convert.ToInt32(DT.Rows[0][2]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IList<IBulkBatchLog> list = repo.GetBatchLogByBatchKeyMessageType(key, mt);
                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetBulkBatchByKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 BulkBatchKey from BulkBatch";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IBulkBatch ibb = repo.GetBulkBatchByKey(key);
                Assert.IsNotNull(ibb);
            }
        }

        [Test]
        public void GetBatchTransactionsByBulkBatchKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 BulkBatchKey, count(BatchTransactionKey) from BatchTransaction group by BulkBatchKey";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IList<IBatchTransaction> list = repo.GetBatchTransactionsByBulkBatchKey(key);
                Assert.That(list.Count == count);

                DataTable dt = repo.GetBatchTransactionByBulkBatchKey(key);
                Assert.Greater(dt.Rows.Count, 0);
                dt = repo.GetBatchTransactionByBulkBatchKey(-100);
                Assert.AreEqual(dt.Rows.Count, 0);
            }
        }


        [Test]
        public void GetSubsidyProviderBySubsidyProviderTypeKey()
        {
            using (new SessionScope())
            {
                string query = @"select count(*) from SubsidyProvider where SubsidyProviderTypeKey = 1";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int count = Convert.ToInt32(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IList<ISubsidyProvider> list = repo.GetSubsidyProviderBySubsidyProviderTypeKey(1);
                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetUpdateBulkTransactionBatchByBulkBatchKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 BulkBatchKey from [2am].[dbo].[BulkBatch] order by 1 desc";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int BulkBatchKey = Convert.ToInt32(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();

                DT = repo.GetUpdateBulkTransactionBatchByBulkBatchKey(BulkBatchKey);

            }
        }

        [Test]
        public void GetAddBulkTransactionBatchBySubsidyProviderKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 S.SubsidyProviderKey from [2AM].[dbo].[Subsidy] S (NOLOCK) "
                    + "JOIN [2AM].[dbo].[LegalEntity] LE (NOLOCK) ON S.LegalEntityKey = LE.LegalEntityKey "
                    + "JOIN [2AM].[dbo].[AccountSubsidy] ACS (NOLOCK) ON S.SubsidyKey = ACS.SubsidyKey "
                    + "JOIN [2AM].[dbo].[Account] A (NOLOCK) ON ACS.AccountKey = A.AccountKey "
                    + "AND A.AccountStatusKey IN (1,5) "
                    + "JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK) ON A.AccountKey = FS.AccountKey "
                    + "AND FS.AccountStatusKey IN (1, 5) "
                    + "JOIN [2AM].[fin].[MortgageLoan] ML (nolock) ON FS.FinancialServiceKey = ML.FinancialServiceKey "
                    + "WHERE S.GeneralStatusKey = 1";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int SubsidyProviderKey = Convert.ToInt32(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();

                DT = repo.GetAddBulkTransactionBatchBySubsidyProviderKey(SubsidyProviderKey, new int[] { 1 });
            }
        }

        [Test]
        public void GetEmptyBatchTransaction()
        {
            using (new SessionScope())
            {
                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IBatchTransaction ibTrans = repo.GetEmptyBatchTransaction();
                Assert.IsNotNull(ibTrans);
            }
        }

        [Test]
        public void GetEmptyBulkBatch()
        {
            using (new SessionScope())
            {
                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IBulkBatch ibBatch = repo.GetEmptyBulkBatch();
                Assert.IsNotNull(ibBatch);
            }
        }

        [Test]
        public void GetEmptyBulkBatchParameter()
        {
            using (new SessionScope())
            {
                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IBulkBatchParameter ibBatchParam = repo.GetEmptyBulkBatchParameter();
                Assert.IsNotNull(ibBatchParam);
            }
        }

    }
}
