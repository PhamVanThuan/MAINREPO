using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class FinancialTransactionTest : TestBase
    {
        [Test]
        public void GetTransactionTypeByKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 TransactionTypeKey from [2am].[fin].TransactionType";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt16(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                ITransactionType itt = repo.GetTransactionTypeByKey(key);
                Assert.IsNotNull(itt);
            }
        }

        [Test]
        public void GetLoanTransactionTypes()
        {
            using (new SessionScope())
            {
                string groups = base.CurrentPrincipalCache.GetCachedRolesAsStringForQuery(true, false);
                string query = "select distinct tt.* "
                            + "from [2am].[fin].TransactionType tt (nolock) "
                            + "join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey "
                            + "join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey "
                            + "join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey "
                            + "join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey "
                            + "join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey "
                            + "where "
                            + "tte.BalanceTypeKey = 1 " // Loan
                            + "and tte.ParentTransactionTypeBalanceEffectKey is null "
                            + "and tt1.ScreenBatch = 1 "
                            + "and ttg.TransactionGroupKey in (1,2) " // -- Financial or Memo
                            + "and ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3) " // Correction
                            + "and ttda.ADCredentials IN (" + groups + ") "
                            + "order by tt.Description asc";

                DataTable DT = base.GetQueryResults(query);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<ITransactionType> list = repo.GetLoanTransactionTypes(base.TestPrincipal);

                Assert.That(list.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetLoanTransactionTypesByKeys()
        {
            using (new SessionScope())
            {
                string groups = base.CurrentPrincipalCache.GetCachedRolesAsStringForQuery(true, false);
                GetLoanTransactionTypesByKeysHelper(groups, "311");
                GetLoanTransactionTypesByKeysHelper(groups, "140, 311");
                GetLoanTransactionTypesByKeysHelper(groups, "120, 140, 311");
            }
        }

        private void GetLoanTransactionTypesByKeysHelper(string groups, string keys)
        {
            string query = "select distinct tt.* "
            + "from [2am].[fin].TransactionType tt (nolock) "
            + "join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey "
            + "join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey "
            + "join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey "
            + "join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey "
            + "join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey "
            + "where tte.TransactionEffectkey < 3  " // Dr & Cr
            + "and tte.BalanceTypeKey = 1 " // Loan
            + "and tt1.ScreenBatch = 1 "
            + "and ttg.TransactionGroupKey in (1,2) " // Financial or Memo
            + "and ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3) " // Correction
            + "and ttda.ADCredentials IN (" + groups + ") and tt.TransactionTypekey IN (" + keys + ") "
            + "order by tt.Description asc";

            DataTable DT = base.GetQueryResults(query);

            IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
            IDomainMessageCollection Messages = new DomainMessageCollection();

            IReadOnlyEventList<ITransactionType> list = repo.GetLoanTransactionTypesByKeys(base.TestPrincipal, keys);

            Assert.That(list.Count == DT.Rows.Count);
        }

        [Test]
        public void GetLoanTransactionTypesArrears()
        {
            using (new SessionScope())
            {
                string groups = base.CurrentPrincipalCache.GetCachedRolesAsStringForQuery(true, false);

                var p = base.CurrentPrincipalCache.Principal;

                string query = @"select distinct tt.*
                                 from [2am].[fin].TransactionType tt (nolock)
                                 join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey
                                 join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
                                 join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                                 join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey
                                 join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey
                                 where tte.TransactionEffectkey < 3 --// Dr & Cr
                                 and tte.BalanceTypeKey = 4 -- // Arrear
                                 and tt1.ScreenBatch = 1
                                 and ttg.TransactionGroupKey = 1 -- // Financial
                                 and ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3) -- Correction
                                 and ttda.ADCredentials IN (" + groups + @")
                                 order by tt.Description asc";
                DataTable DT = base.GetQueryResults(query);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                //IReadOnlyEventList<ITransactionType> list = repo.GetLoanTransactionTypesArrears(base.TestPrincipal);

                IReadOnlyEventList<ITransactionType> list = repo.GetLoanTransactionTypesArrears(base.CurrentPrincipalCache.Principal);

                Assert.That(list.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetTransactionTypeGroup()
        {
            using (new SessionScope())
            {
                string query = @"select * from [2am].[fin].TransactionType tt (nolock)
                                        join [2am].[fin].TransactionTypeGroup ttg (nolock) on ttg.TransactionTypeKey = tt.TransactionTypeKey
                                        where ttg.TransactionGroupKey = 1 "; // financial
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt16(DT.Rows[0][0]);

                IBulkBatchRepository repo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                ITransactionType itt = repo.GetTransactionTypeByKey(key);
                Assert.IsNotNull(itt);
                Assert.IsTrue(itt.TransactionGroups[0].Key == (int)SAHL.Common.Globals.TransactionGroups.Financial);
            }
        }
    }
}