using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IBankingDetailsService
    {
        void RemoveBankAccount(string CDVaccountNumber);

        Automation.DataModels.BankAccount GetBankAccountByBankAccountKey(int bankAccountKey);

        Automation.DataModels.LegalEntityBankAccount GetRandomLegalEntityBankAccountByBankAccountKey();

        List<string> GetBankAccountStringList(List<Automation.DataModels.BankAccount> bankAccounts);

        QueryResults GetBankAccount(string bank, string branchCode, string accountType, string accountNumber);

        QueryResults GetLegalEntityBankAccountByStatus(int legalEntityKey, int bankAccountKey, GeneralStatusEnum status);

        Automation.DataModels.BankAccount GetNextUnusedBankAccountDetails();

        string GetBankAccountString(string type, int offerKey);
    }
}