using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ManualDebitOrderRepositoryTests : TestBase
    {
        private static readonly IManualDebitOrderRepository _manDebitOrderRepo = RepositoryFactory.GetRepository<IManualDebitOrderRepository>();

        [SetUp()]
        public void Setup()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [Test]
        public void GetEmptyFinancialServiceRecurringTransaction()
        {
            using (new SessionScope())
            {
                IManualDebitOrder manDebitOrder = _manDebitOrderRepo.GetEmptyManualDebitOrder();
                Assert.IsNotNull(manDebitOrder);
            }
        }

        [Test]
        public void TestCreateManualDebitOrder()
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IBankAccountRepository bankAccountRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IMemoRepository memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();

            string query = @"select top 1 FinancialServiceKey, fs.Payment, leba.BankAccountKey
                             from FinancialService fs
                             join [Role] r (nolock) on fs.AccountKey = r.AccountKey
                             join LegalEntityBankAccount leba (nolock) on r.LegalEntityKey = leba.LegalEntityKey
                             where FinancialServiceTypeKey = 1 and AccountStatusKey = 1 and Payment > 1000";

            ParameterCollection emptyParams = new DataAccess.ParameterCollection();

            DataSet data = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), emptyParams);

            if (data.Tables.Count < 1 || data.Tables[0].Rows.Count < 1)
            {
                Assert.Fail("No data for TestCreateManualDebitOrder Test");
            }

            int fsKey = Convert.ToInt32(data.Tables[0].Rows[0]["FinancialServiceKey"]);
            double amount = Convert.ToDouble(data.Tables[0].Rows[0]["Payment"]);
            int bankAccountKey = Convert.ToInt32(data.Tables[0].Rows[0]["BankAccountKey"]);
            int memoKey = Convert.ToInt32(CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran("select top 1 MemoKey from Memo (nolock)", typeof(GeneralStatus_DAO), emptyParams));

            string user = SAHLPrincipal.GetCurrent().Identity.Name;

            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime actionDate = new DateTime(nextMonth.Year, nextMonth.Month, 25);

            TransactionScope tx = new TransactionScope();
            try
            {
                IFinancialService financialService = financialServiceRepo.GetFinancialServiceByKey(fsKey);
                IBankAccount bankAccount = bankAccountRepo.GetBankAccountByKey(bankAccountKey);
                IMemo memo = memoRepository.GetMemoByKey(memoKey);
                ITransactionType transactionType = lookupRepo.TransactionTypes.First(x => x.Key == (int)TransactionTypes.ManualDebitOrderPayment);

                IManualDebitOrder manualDebitOrder = _manDebitOrderRepo.GetEmptyManualDebitOrder();
                manualDebitOrder.ActionDate = actionDate;
                manualDebitOrder.Amount = amount;
                manualDebitOrder.BankAccount = bankAccount;
                manualDebitOrder.FinancialService = financialService;
                manualDebitOrder.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                manualDebitOrder.InsertDate = DateTime.Now;
                manualDebitOrder.Memo = memo;
                manualDebitOrder.Reference = "Test";
                manualDebitOrder.TransactionType = transactionType;
                manualDebitOrder.UserID = user;

                _manDebitOrderRepo.SaveManualDebitOrder(manualDebitOrder);

                IManualDebitOrder newlyCreated = _manDebitOrderRepo.GetManualDebitOrderByKey(manualDebitOrder.Key);

                Assert.NotNull(newlyCreated);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                tx.VoteRollBack();
            }
            finally
            {
                tx.VoteRollBack();
                tx.Dispose();
            }
        }

        [Test]
        public void TestCancelManualDebitOrder()
        {
            string HQL = "from ManualDebitOrder_DAO mdo where mdo.GeneralStatus.Key= ?";
            SimpleQuery<ManualDebitOrder_DAO> simpleQuery = new SimpleQuery<ManualDebitOrder_DAO>(HQL, 1);
            ManualDebitOrder_DAO[] manDebitOrder_Table = simpleQuery.Execute();

            if (manDebitOrder_Table.Length == 0)
            {
                Assert.Ignore("No data to perform test");
            }

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IManualDebitOrder manualDebitOrder = typeMapper.GetMappedType<IManualDebitOrder, ManualDebitOrder_DAO>(manDebitOrder_Table[0]);

            TransactionScope tx = new TransactionScope();
            try
            {
                _manDebitOrderRepo.CancelManualDebitOrder(manualDebitOrder);

                Assert.That(manualDebitOrder.GeneralStatus.Key == (int)GeneralStatuses.Inactive);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                tx.VoteRollBack();
                tx.Dispose();
            }
        }

        [Test]
        public void TestGetPendingManualDebitOrdersByFinancialServiceKey()
        {
            var query = @"  DECLARE @financialServiceKey INT
                            DECLARE @pendingCount INT

                            SELECT @financialServiceKey = FinancialServiceKey, @pendingCount = Pending
                            FROM
                            (
	                            SELECT TOP 1 mdo.FinancialServiceKey AS FinancialServiceKey, count(*) AS Pending
	                            FROM deb.ManualDebitOrder mdo (NOLOCK)
	                            LEFT JOIN deb.BatchTotal batTot (NOLOCK) ON mdo.BatchTotalKey = batTot.BatchTotalKey
	                            LEFT JOIN deb.DOTransaction dot (NOLOCK) ON batTot.BatchTotalKey = dot.BatchTotalKey AND batTot.AccountKey = dot.AccountKey AND dot.TransactionStatusKey < 4
	                            WHERE mdo.TransactionTypeKey = 710
	                            AND ( mdo.generalStatuskey = 1 or dot.DotransactionKey is not null )
	                            GROUP BY mdo.FinancialServiceKey
	                            ORDER BY count(*) DESC
                            ) pending

                            select @financialServiceKey, @pendingCount";

            DataTable DT = base.GetQueryResults(query);

            if (DT.Rows[0][0] == DBNull.Value || DT.Rows[0][1] == DBNull.Value)
            {
                Assert.Ignore("Insufficient Data to perform this test.");
            }

            int financialServiceKey = Convert.ToInt32(DT.Rows[0][0]);
            int pendingCount = Convert.ToInt32(DT.Rows[0][1]);

            var listPending = _manDebitOrderRepo.GetPendingManualDebitOrdersByFinancialServiceKey(financialServiceKey);

            Assert.That(listPending.Count == pendingCount, String.Format("FSKey: {0}, Pending: {1}", financialServiceKey.ToString(), pendingCount.ToString()));
        }

        [Test]
        public void TestGetCollectedManualDebitOrderList()
        {
            var query = @"  select top 1 mdo.FinancialServiceKey, count(*)
                            from [2am].deb.ManualDebitOrder as mdo (nolock)
                            join [2am].deb.BatchTotal as batTot (nolock) on mdo.BatchTotalKey = batTot.BatchTotalKey
                            join [2am].deb.DOTransaction as dot on batTot.BatchTotalKey = dot.BatchTotalKey and batTot.AccountKey = dot.AccountKey
                            where mdo.TransactionTypeKey = 710 and dot.TransactionStatusKey = 4
                            group by mdo.FinancialServiceKey
                            having count(*) > 1
                            order by mdo.FinancialServiceKey";

            DataTable DT = base.GetQueryResults(query);

            if (DT.Rows.Count == 0)
            {
                Assert.Ignore("Insufficient Data to perform this test.");
            }

            int financialServiceKey = Convert.ToInt32(DT.Rows[0][0]);
            int count = Convert.ToInt32(DT.Rows[0][1]);

            var manualDebitOrders = _manDebitOrderRepo.GetCollectedManualDebitOrdersByFinancialServiceKey(financialServiceKey);

            Assert.That(manualDebitOrders.Count == count);
        }
    }
}