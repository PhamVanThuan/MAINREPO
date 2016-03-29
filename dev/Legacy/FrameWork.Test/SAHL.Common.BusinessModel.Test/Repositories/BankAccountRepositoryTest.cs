using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class BankAccountRepositoryTest : TestBase
    {
        private IBankAccountRepository _repo = RepositoryFactory.GetRepository<IBankAccountRepository>();

        [Test]
        public void GetBankAccountByKey()
        {
            using (new SessionScope())
            {
                string acbBankKey = base.GetPrimaryKey("BankAccount", "BankAccountKey").ToString();
                IBankAccount bank1 = _repo.GetBankAccountByKey(int.Parse(acbBankKey));
                Assert.IsNotNull(bank1);

                //and save
                _repo.SaveBankAccount(bank1);
            }
        }

        [Test]
        public void GetBankAccountByACBBranchCodeAndAccountNumber()
        {
            string sql = @"select top 1 bankaccountkey,acbbranchcode,accountnumber from BankAccount";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            int bankAccountKey = Convert.ToInt32(dt.Rows[0][0]);
            string branchcode = dt.Rows[0][1].ToString();
            string accountnumber = dt.Rows[0][2].ToString();
            dt.Dispose();

            using (new SessionScope(FlushAction.Never))
            {
                IBankAccount ba = _repo.GetBankAccountByACBBranchCodeAndAccountNumber(branchcode.ToString(), accountnumber.ToString());
                Assert.IsNotNull(ba);
            }
        }

        [Test]
        public void GetACBBranchByKey()
        {
            using (new SessionScope())
            {
                string acbBranchKey = base.GetPrimaryKey("ACBBranch", "ACBBranchCode").ToString();
                IACBBranch branch1 = _repo.GetACBBranchByKey(acbBranchKey);
                Assert.IsNotNull(branch1);

                IACBBranch branch2 = _repo.GetACBBranchByKey("notaprimarykey");
                Assert.IsNull(branch2);
            }
        }

        /// <summary>
        /// Tests the search function for legal entity bank accounts.
        /// </summary>
        [Test]
        public void SearchLegalEntityBankAccounts()
        {
            using (new SessionScope(FlushAction.Never))
            {
                // retrieve an example to search for
                int key = Convert.ToInt32(base.GetPrimaryKey("LegalEntityBankAccount", "LegalEntityBankAccountKey"));
                LegalEntityBankAccount_DAO leBankAccount = LegalEntityBankAccount_DAO.Find(key);
                string branchKey = leBankAccount.BankAccount.ACBBranch.Key;

                // set up the search criteria and perform the search
                IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();
                searchCriteria.ACBBranchKey = branchKey;
                searchCriteria.AccountName = leBankAccount.BankAccount.AccountName;
                searchCriteria.AccountNumber = leBankAccount.BankAccount.AccountNumber;
                IBankAccountRepository repository = RepositoryFactory.GetRepository<IBankAccountRepository>();
                IEventList<ILegalEntityBankAccount> bankAccounts = repository.SearchLegalEntityBankAccounts(searchCriteria, -1);

                // ensure that the bank account above is found
                bool found = false;
                foreach (ILegalEntityBankAccount bankAccount in bankAccounts)
                {
                    if (bankAccount.Key == key)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsTrue(found, "LegalEntityBankAccount [" + key.ToString() + "] not found in search.");
            }
        }

        [Test]
        public void GetACBBranchesByPrefix()
        {
            string sql = @"select top 1 b.ACBBankCode, b.ACBBranchDescription
                from ACBBranch b (nolock)
                where ACBBankCode > 0
                and LEN(ACBBranchDescription) > 2";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");

            int acbBankCode = Convert.ToInt32(dt.Rows[0]["ACBBankCode"]);
            string prefix = dt.Rows[0]["ACBBranchDescription"].ToString().Substring(0, 1).ToLower();
            dt.Dispose();

            using (new SessionScope(FlushAction.Never))
            {
                IReadOnlyEventList<IACBBranch> list = _repo.GetACBBranchesByPrefix(acbBankCode, prefix, 1);
                Assert.AreEqual(list.Count, 1);
                Assert.IsTrue(list[0].ACBBranchDescription.ToLower().StartsWith(prefix.ToLower()));
            }
        }

        [Test]
        public void GetACBBankByKeyTest()
        {
            int acbBankKey = Convert.ToInt32(GetPrimaryKey("ACBBank", "ACBBankCode"));

            using (new SessionScope(FlushAction.Never))
            {
                IACBBank a = _repo.GetACBBankByKey(acbBankKey);
                Assert.IsNotNull(a);
            }
        }

        [Test]
        public void GetACBBranchByKeyTest()
        {
            string acbBranchCode = GetPrimaryKey("ACBBranch", "ACBBranchCode").ToString();

            using (new SessionScope(FlushAction.Never))
            {
                IACBBranch b = _repo.GetACBBranchByKey(acbBranchCode);
                Assert.IsNotNull(b);
            }
        }

        [Test]
        public void GetACBTypeByKeyTest()
        {
            int acbTypeKey = Convert.ToInt32(GetPrimaryKey("ACBType", "ACBTypeNumber"));

            using (new SessionScope(FlushAction.Never))
            {
                IACBType t = _repo.GetACBTypeByKey(acbTypeKey);
                Assert.IsNotNull(t);
            }
        }

        [Test]
        public void GetEmptyBankAccountTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IBankAccount ba = _repo.GetEmptyBankAccount();
                Assert.IsNotNull(ba);
            }
        }

        [Test]
        public void DeleteApplicationDebtSettlementBankAccountTest()
        {
            int adsKey = 0;

            // create a new account debt settlement object
            using (new SessionScope())
            {
                ApplicationDebtSettlement_DAO ads = DAODataConsistancyChecker.GetDAO<ApplicationDebtSettlement_DAO>();
                ads.BankAccount = BankAccount_DAO.FindFirst(); ;
                ads.SaveAndFlush();
                adsKey = ads.Key;
            }

            try
            {
                // call the delete method on the new object
                using (new SessionScope())
                {
                    _repo.DeleteApplicationDebtSettlementBankAccount(adsKey);
                }

                // in a new session, load up the object and assert that the bank account value is null
                using (new SessionScope(FlushAction.Never))
                {
                    ApplicationDebtSettlement_DAO ads = ApplicationDebtSettlement_DAO.Find(adsKey);
                    Assert.IsNull(ads.BankAccount);
                }
            }
            finally
            {
                using (new SessionScope())
                {
                    ApplicationDebtSettlement_DAO ads = ApplicationDebtSettlement_DAO.Find(adsKey);
                    ads.DeleteAndFlush();
                }
            }
        }

        /// <summary>
        /// Tests the SearchNonLegalEntityBankAccounts
        /// </summary>
        [Test]
        public void SearchNonLegalEntityBankAccounts()
        {
            string sql = @"select top 1 ba.ACBBranchCode, ba.AccountNumber
                from BankAccount ba (nolock)
                where ba.ACBBranchCode > 0
                and ba.ACBTypeNumber is not null
                and ba.BankAccountKey not in (select leba.BankAccountKey from LegalEntityBankAccount leba (nolock))";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");

            string acbBranchCode = dt.Rows[0]["ACBBranchCode"].ToString();
            string accountNumber = dt.Rows[0]["AccountNumber"].ToString();

            dt.Dispose();

            using (new SessionScope(FlushAction.Never))
            {
                IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();

                // branch code
                searchCriteria.ACBBranchKey = acbBranchCode;
                searchCriteria.AccountNumber = accountNumber;

                IEventList<IBankAccount> bankAccounts = _repo.SearchNonLegalEntityBankAccounts(searchCriteria, 1);
                Assert.AreEqual(bankAccounts.Count, 1);
                Assert.AreEqual(bankAccounts[0].ACBBranch.Key, acbBranchCode);
                Assert.AreEqual(bankAccounts[0].AccountNumber, accountNumber);
            }
        }
    }
}