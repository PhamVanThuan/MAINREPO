using SAHL.Core.Data.Models._2AM;
using SAHL.Services.CATS.Models;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Managers.CATS
{
    public interface ICATSManager
    {
        List<DetailedPayment> GenerateDetailedPayment(IEnumerable<CATSPaymentBatchItemDataModel> batchItem);

        List<PaymentBatch> GroupPaymentsBySourceandThenTargetBankAccounts(List<DetailedPayment> disbursement);

        BankAccountDataModel ApplyCATsFormat(BankAccountDataModel bankAccount);

        String TimestampFileName(String fileName);

        bool HasFileFailedProcessing(string filename);

        bool IsThereACatsFileBeingProcessedForProfile(string catsProfile);

        List<Payment> AddPaymentToPaymentsListSummedByTargetBankAccount(List<Payment> payments, DetailedPayment detailedPayment, string reference);

        IDictionary<int, IEnumerable<CATSPaymentBatchItemDataModel>> GroupBatchPaymentsByRecipient(IEnumerable<CATSPaymentBatchItemDataModel> batchPayments);
    }
}
