using SAHL.Core.Data.Models._2AM;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Models;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.CATS.Managers.CATS
{
    public class CATSManager : ICATSManager
    {
        private ICATSDataManager CatsDataManager;
        private IFileSystem fileSystem;
        private ICatsAppConfigSettings catsConfigSettings;

        public CATSManager(ICATSDataManager catsDataManager, IFileSystem fileSystem, ICatsAppConfigSettings catsConfigSettings)
        {
            this.CatsDataManager = catsDataManager;
            this.fileSystem = fileSystem;
            this.catsConfigSettings = catsConfigSettings;
        }

        public List<DetailedPayment> GenerateDetailedPayment(IEnumerable<CATSPaymentBatchItemDataModel> batchItems)
        {
            var results = new List<DetailedPayment>();
            foreach (var item in batchItems)
            {
                var sourceBankAccount = CatsDataManager.GetBankingAccountByKey(item.SourceBankAccountKey);
                sourceBankAccount = ApplyCATsFormat(sourceBankAccount);
                var targetBankAccount = CatsDataManager.GetBankingAccountByKey(item.TargetBankAccountKey);
                targetBankAccount = ApplyCATsFormat(targetBankAccount);
                var paymentTran = new DetailedPayment(item, sourceBankAccount, targetBankAccount);
                results.Add(paymentTran);
            }
            return results;
        }

        public List<PaymentBatch> GroupPaymentsBySourceandThenTargetBankAccounts(List<DetailedPayment> paymentTrans)
        {
            var externalReference = "SA Home Loans";
            var result = new List<PaymentBatch>();
            var groupedPaymentTran = paymentTrans.GroupBy(x => x.SourceBankAccountKey);
            foreach (var paymentTran in groupedPaymentTran)
            {
                var paymentBatchamount = 0.0m;
                var batchPayments = new List<Payment>();
                BankAccountDataModel sourceAccount = paymentTran.FirstOrDefault().SourceBankAccount;
                var sourceReference = paymentTran.FirstOrDefault().SourceReferenceNumber;
                foreach (var detailedPaymentTran in paymentTran)
                {
                    paymentBatchamount += (decimal)detailedPaymentTran.Amount;
                    batchPayments = AddPaymentToPaymentsListSummedByTargetBankAccount(batchPayments, detailedPaymentTran, externalReference);
                }
                var paymentBatch = new PaymentBatch(batchPayments, sourceAccount, paymentBatchamount, sourceReference);
                result.Add(paymentBatch);
            }
            return result;
        }

        public List<Payment> AddPaymentToPaymentsListSummedByTargetBankAccount(List<Payment> payments, DetailedPayment detailedPayment, string reference)
        {

            if (!payments.Any(x => x.TargetAccount.AccountNumber.Equals(detailedPayment.TargetBankAccount.AccountNumber)
                && x.TargetAccount.ACBBranchCode.Equals(detailedPayment.TargetBankAccount.ACBBranchCode)))
            {
                var payment = new Payment(detailedPayment.TargetBankAccount, (Decimal)detailedPayment.Amount, reference);
                payments.Add(payment);
            }
            else
            {
                var payment = payments.Where(x => x.TargetAccount.AccountNumber.Equals(detailedPayment.TargetBankAccount.AccountNumber)).FirstOrDefault();
                payments = payments.Where(x => x.TargetAccount.AccountNumber != detailedPayment.TargetBankAccount.AccountNumber).ToList();
                payment = new Payment(detailedPayment.TargetBankAccount, payment.Amount + (Decimal)detailedPayment.Amount, reference);
                payments.Add(payment);
            }
            return payments;
        }

        public BankAccountDataModel ApplyCATsFormat(BankAccountDataModel bankAccount)
        {
            var formatedAccount = bankAccount;
            if (bankAccount.AccountName.Length > 30)
            {
                var tractatedAccountName = bankAccount.AccountName.Substring(0, 30);
                formatedAccount = new BankAccountDataModel(
                    bankAccount.ACBBranchCode, bankAccount.AccountNumber, bankAccount.ACBTypeNumber, tractatedAccountName, bankAccount.UserID, bankAccount.ChangeDate);
            }
            return formatedAccount;
        }

        public String TimestampFileName(String fileName)
        {
            return string.Format("{0}_{1}", fileName, DateTime.Now.ToString("yyyyMMddHmmss")); ;
        }


        public bool HasFileFailedProcessing(string filename)
        {
            return (fileSystem.File.Exists(catsConfigSettings.CATSFailureFileLocation + filename)
                    || fileSystem.File.Exists(catsConfigSettings.CATSOutputFileLocation + filename)
                );
        }

        public bool IsThereACatsFileBeingProcessedForProfile(string catsProfile)
        {
            return (fileSystem.Directory.EnumerateFileSystemEntries(catsConfigSettings.CATSFailureFileLocation, catsProfile + "*").Any()
                    || fileSystem.Directory.EnumerateFileSystemEntries(catsConfigSettings.CATSOutputFileLocation, catsProfile + "*").Any()
                );
        }

        public IDictionary<int, IEnumerable<CATSPaymentBatchItemDataModel>> GroupBatchPaymentsByRecipient(IEnumerable<CATSPaymentBatchItemDataModel> batchPayments)
        {
            var groupedPayments = batchPayments.GroupBy(x => x.LegalEntityKey).ToDictionary(g => g.Key, d => d.ToList() as IEnumerable<CATSPaymentBatchItemDataModel>);
            return groupedPayments;
        }
    }
}
