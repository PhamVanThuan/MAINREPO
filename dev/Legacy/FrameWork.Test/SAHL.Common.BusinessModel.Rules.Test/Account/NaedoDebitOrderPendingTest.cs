using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.Account;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Factories;
using System;

namespace SAHL.Common.BusinessModel.Rules.Test.Account
{
    [TestFixture]
    public class NaedoDebitOrderPendingTest : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void NaedoDebitOrderPendingTestFail()
        {
            string sql = @" select top 1 dt.AccountKey
                from [2am].deb.Batch b (nolock)
                join [2am].deb.DOTransaction dt (nolock) on b.BatchKey = dt.BatchKey
                where b.ProviderKey = 3				-- Naedo
	                and b.TransactionTypeKey = 310		-- Debit Order (not manual)
	                and dt.TransactionStatusKey in (2, 3)
	                and isnull(dt.ErrorKey, 128) = 128 ";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                var accountKey = Convert.ToInt32(obj);

                IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                var account = accountRepository.GetAccountByKey(accountKey);

                var castleTransactionService = new CastleTransactionsService();
                NaedoDebitOrderPending rule = new NaedoDebitOrderPending(castleTransactionService);
                ExecuteRule(rule, 1, account);
            }
        }

        [Test]
        public void NaedoDebitOrderPendingTestPass()
        {
            string sql = @"select top 1 dt.AccountKey
                from [2am].deb.Batch b (nolock)
                join [2am].deb.DOTransaction dt (nolock) on b.BatchKey = dt.BatchKey
                where b.ProviderKey = 2				-- Connect Direct
                and dt.AccountKey not in (
		                select distinct dt.AccountKey
						                from [2am].deb.Batch b (nolock)
						                join [2am].deb.DOTransaction dt (nolock) on b.BatchKey = dt.BatchKey
						                where b.ProviderKey = 3				-- Naedo
							                and b.TransactionTypeKey = 310		-- Debit Order (not manual)
							                and dt.TransactionStatusKey in (2, 3)
							                and isnull(dt.ErrorKey, 128) = 128)";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                var accountKey = Convert.ToInt32(obj);

                IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                var account = accountRepository.GetAccountByKey(accountKey);

                var castleTransactionService = new CastleTransactionsService();
                NaedoDebitOrderPending rule = new NaedoDebitOrderPending(castleTransactionService);
                ExecuteRule(rule, 0, account);
            }
        }
    }
}