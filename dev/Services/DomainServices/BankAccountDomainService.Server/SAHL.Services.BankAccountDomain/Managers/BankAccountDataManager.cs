using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Managers
{
    public class BankAccountDataManager : IBankAccountDataManager
    {
        private IDbFactory dbFactory;
        public BankAccountDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<BankAccountDataModel> FindExistingBankAccount(BankAccountModel bankAccount)
        {
            IEnumerable<BankAccountDataModel> bankAccounts = Enumerable.Empty<BankAccountDataModel>();
            var getBankAccountQuery = new GetBankAccountStatement(bankAccount);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                bankAccounts = db.Select<BankAccountDataModel>(getBankAccountQuery);
            }
            return bankAccounts;
        }

        public int SaveBankAccount(BankAccountModel bankAccount)
        {
            var newBankAccount = new BankAccountDataModel(bankAccount.BranchCode, bankAccount.AccountNumber, (int)bankAccount.AccountType, bankAccount.AccountName, "System", DateTime.Now);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<BankAccountDataModel>(newBankAccount);
                db.Complete();
                return newBankAccount.BankAccountKey;
            }
        }

        public int AddBankAccountToClient(int clientKey, int bankAccountKey)
        {
            var clientBankAccount = new LegalEntityBankAccountDataModel(clientKey, bankAccountKey, (int)GeneralStatus.Active, "System", DateTime.Now);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<LegalEntityBankAccountDataModel>(clientBankAccount);
                db.Complete();
                return clientBankAccount.LegalEntityBankAccountKey;
            }
        }

        public IEnumerable<LegalEntityBankAccountDataModel> FindExistingClientBankAccount(int clientKey, int bankAccountKey)
        {
            IEnumerable<LegalEntityBankAccountDataModel> clientBankAccounts = Enumerable.Empty<LegalEntityBankAccountDataModel>();
            var getClientBankAccountQuery = new GetClientBankAccountsStatement(clientKey, bankAccountKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                clientBankAccounts = db.Select<LegalEntityBankAccountDataModel>(getClientBankAccountQuery);
            }

            return clientBankAccounts;
        }

        public void ReactivateClientBankAccount(int clientBankAccountKey)
        {
            var updateStatusQuery = new UpdateClientBankAccountStatusStatement(clientBankAccountKey, GeneralStatus.Active);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<LegalEntityBankAccountDataModel>(updateStatusQuery);
                db.Complete();
            }
        }

        public IEnumerable<ACBBranchDataModel> FindExistingBranch(BankAccountModel bankAccountModel)
        {
            IEnumerable<ACBBranchDataModel> existingBranches = Enumerable.Empty<ACBBranchDataModel>();
            var getBranchQuery = new GetBranchStatement(bankAccountModel.BranchCode);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                existingBranches = db.Select<ACBBranchDataModel>(getBranchQuery);
            }
            return existingBranches;
        }
    }
}