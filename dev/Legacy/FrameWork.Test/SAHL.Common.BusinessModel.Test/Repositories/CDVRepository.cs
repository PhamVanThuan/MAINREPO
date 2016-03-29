using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class CDVRepositoryTest : TestBase
    {
        [NUnit.Framework.Test]
        public void CDV_Validation_Pass()
        {
            using (new SessionScope())
            {
                IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();

                // find a valid bank account record
                int bankAccountKey;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = "select top 1 * from [2am]..BankAccount with (nolock) where AccountNumber <> '' and ACBTypeNumber = 1 ";
                    bankAccountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                IBankAccount bankAccount = bankRepo.GetBankAccountByKey(bankAccountKey);

                string acbBranchCode = bankAccount.ACBBranch.Key;
                int acbTypeNumber = bankAccount.ACBType.Key;
                string accountNumber = bankAccount.AccountNumber;

                ICDVRepository cr = RepositoryFactory.GetRepository<ICDVRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                bool result = cr.ValidateAccountNo(acbBranchCode, acbTypeNumber, accountNumber);

                Assert.IsTrue(result == true);
            }
        }

        [NUnit.Framework.Test]
        public void CDV_Validation_Fail()
        {
            using (new SessionScope())
            {
                IBankAccountRepository bankRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();

                // find a valid bank account record
                int bankAccountKey;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = "select top 1 * from [2am]..BankAccount with (nolock) where AccountNumber <> '' and ACBTypeNumber = 1 ";
                    bankAccountKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                IBankAccount bankAccount = bankRepo.GetBankAccountByKey(bankAccountKey);

                string acbBranchCode = bankAccount.ACBBranch.Key;
                int acbTypeNumber = bankAccount.ACBType.Key;

                // set the account number to an invalid value
                string accountNumber = "9999999999999999999999";

                ICDVRepository cr = RepositoryFactory.GetRepository<ICDVRepository>();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                bool result = cr.ValidateAccountNo(acbBranchCode, acbTypeNumber, accountNumber);

                Assert.IsTrue(result == false);
            }
        }
    }
}