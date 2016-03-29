using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILegalEntityBankAccountService
    {
        IEnumerable<Automation.DataModels.LegalEntityBankAccount> GetLegalEntityBankAccount(int legalEntityKey);

        IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntitiesLinkedToABankAccount(int bankAccountKey);

        IEnumerable<Automation.DataModels.BankAccount> GetBankAccountsLinkedToALegalEntities(int legalEntityKey);

        IEnumerable<Automation.DataModels.LegalEntityBankAccount> GetLegalEntityBankAccount();
    }
}