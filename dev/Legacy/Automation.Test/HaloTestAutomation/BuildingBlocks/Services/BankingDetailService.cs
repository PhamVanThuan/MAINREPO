using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class BankingDetailService : _2AMDataHelper, IBankingDetailsService
    {
        /// <summary>
        /// Get BankAccount details by BankAccountKey
        /// </summary>
        /// <param name="bankAccountKey">BankAccountKey</param>
        /// <returns>Models.BankAccountModel</returns>
        public Automation.DataModels.BankAccount GetBankAccountByBankAccountKey(int bankAccountKey)
        {
            var bankAccount = (from bank in base.GetBankAccount(bankAccountKey) select bank).FirstOrDefault();
            return bankAccount;
        }

        /// <summary>
        /// Get Random LegalEntityBankAccount record
        /// </summary>
        /// <returns>Models.LegalEntityBankAccount</returns>
        public Automation.DataModels.LegalEntityBankAccount GetRandomLegalEntityBankAccountByBankAccountKey()
        {
            IEnumerable<Automation.DataModels.LegalEntityBankAccount> legalEntityBankAccount = base.GetLegalEntityBankAccount();
            Random num = new Random();
            int i = num.Next(0, legalEntityBankAccount.Count());
            return (from l in legalEntityBankAccount select l).ElementAt(i);
        }

        /// <summary>
        /// Gets a formatted string list of bank accounts.
        /// </summary>
        /// <param name="bankAccounts">Bank Account Model</param>
        /// <returns>Formatted list if Bank Accounts</returns>
        public List<string> GetBankAccountStringList(List<Automation.DataModels.BankAccount> bankAccounts)
        {
            return bankAccounts.Select(b => string.Format(@"{0} - {1} - {2} - {3} - {4} - {5}",
                b.ACBBankDescription.Trim(), b.ACBBranchCode.Trim(), b.ACBBranchDescription.Trim(), b.ACBTypeNumber, b.AccountNumber.Trim(), b.AccountName.Trim() ?? string.Empty)).ToList();
        }

        public Automation.DataModels.BankAccount GetNextUnusedBankAccountDetails()
        {
            var results = base.GetUnusedBankAccountDetails();
            var bankAcc = new Automation.DataModels.BankAccount
            {
                ACBBankDescription = results.Rows(0).Column(0).Value,
                ACBBranchCode = results.Rows(0).Column(1).Value,
                AccountNumber = results.Rows(0).Column(3).Value,
                ACBTypeDescription = results.Rows(0).Column(5).Value
            };
            results.Dispose();
            base.UpdateBankAccountDetailsStatus(bankAcc.AccountNumber, 1);
            return bankAcc;
        }

        /// <summary>
        /// This method will get the bank account details for the client and the SAHL valuation bank account so that the
        /// screen can select the bank account from the dropdown
        /// </summary>
        /// <param name="type">If you require the SAHL Valuation A/C then Type = Valuations</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>BankAccount Details String</returns>
        public string GetBankAccountString(string type, int offerKey)
        {
            var bankaccounts = base.GetDisbursementBankAccountsByOfferKey(offerKey);
            string bankAccount;
            if (type == "Valuations")
            {
                bankAccount = (from b in bankaccounts
                               where b.Column("AccountType").Value == "SAHL Valuation"
                               select b.Column("BankDetails").Value).FirstOrDefault();
            }
            else
            {
                bankAccount = (from b in bankaccounts
                               where b.Column("AccountType").Value == "Client"
                               select b.Column("BankDetails").Value).FirstOrDefault();
            }
            return bankAccount;
        }
    }
}