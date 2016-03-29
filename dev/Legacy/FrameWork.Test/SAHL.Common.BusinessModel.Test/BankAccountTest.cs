using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Data;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class BankAccountTest : TestBase
    {
        /// <summary>
        /// Ensures that BankAccount.GetFinancialServiceBankAccounts works.
        /// </summary>
        [Test]
        public void GetFinancialServiceBankAccounts()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 BankAccountKey FROM [2AM].[dbo].[FinancialServiceBankAccount] f (nolock) WHERE f.BankAccountKey IS NOT NULL";
                DataTable DT = base.GetQueryResults(query);
                int bankAccountKey = Convert.ToInt32(DT.Rows[0][0]);

                BankAccount_DAO baDao = BankAccount_DAO.Find(bankAccountKey);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IBankAccount bankAccount = BMTM.GetMappedType<IBankAccount>(baDao);
                IReadOnlyEventList<IFinancialServiceBankAccount> result = bankAccount.GetFinancialServiceBankAccounts(1);
                Assert.AreEqual(result.Count, 1);
            }
        }

        /// <summary>
        /// Ensures that BankAccount.GetFinancialServiceBankAccounts without an argument only returns 50 records.
        /// </summary>
        [Test]
        public void GetFinancialServiceBankAccountsMaximumRecordCheck()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 bankaccountkey as a 
                    from financialservicebankaccount 
                    where bankaccountkey is not null 
                    group by bankaccountkey 
                    having count(bankaccountkey) > 50
                    ";
                DataTable DT = base.GetQueryResults(query);
                int bankAccountKey = Convert.ToInt32(DT.Rows[0][0]);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                BankAccount_DAO baDao = BankAccount_DAO.Find(bankAccountKey);
                IBankAccount bankAccount = BMTM.GetMappedType<IBankAccount>(baDao);
                IReadOnlyEventList<IFinancialServiceBankAccount> result = bankAccount.GetFinancialServiceBankAccounts();
                Assert.AreEqual(result.Count, 50);
            }
        }

    }
}
