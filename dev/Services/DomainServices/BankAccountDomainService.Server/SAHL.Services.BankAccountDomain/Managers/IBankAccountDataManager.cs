using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.BankAccountDomain.Managers
{
    public interface IBankAccountDataManager
    {
        IEnumerable<BankAccountDataModel> FindExistingBankAccount(BankAccountModel bankAccount);

        int SaveBankAccount(BankAccountModel bankAccount);

        int AddBankAccountToClient(int clientKey, int bankAccountKey);

        IEnumerable<LegalEntityBankAccountDataModel> FindExistingClientBankAccount(int clientKey, int bankAccountKey);

        void ReactivateClientBankAccount(int clientBankAccountKey);

        IEnumerable<ACBBranchDataModel> FindExistingBranch(BankAccountModel bankAccountModel);
    }
}
