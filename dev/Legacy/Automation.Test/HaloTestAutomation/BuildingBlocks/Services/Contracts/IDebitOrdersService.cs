using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IDebitOrdersService
    {
        void InsertFirstLegalEntityBankAccountAsFSBankAccount(int accountKey, FinancialServicePaymentTypeEnum paymentType);

        IEnumerable<Automation.DataModels.FinancialServiceBankAccountModel> GetDebitOrderFinancialServiceBankAccount(int debitOrderDay, bool isNaedoCompliant);

        bool UpdateFinancialServiceBankAccountStatus(int accountKey, GeneralStatusEnum generalStatus);

        void DeleteOfferDebitOrderByBankAccountKey(int _bankAccountKey);

        IEnumerable<Automation.DataModels.FinancialServiceBankAccountModel> GetFinancialServiceBankAccounts(int accountKey);

        void DeleteOfferDebitOrderByOffer(int offerKey);

        IEnumerable<Automation.DataModels.DOTransaction> GetAccountWithinNaedoTrackingPeriod();

        void UpdateAccountOnDOTransactionRecord(int accountKey, int DOTransactionKey);
    }
}