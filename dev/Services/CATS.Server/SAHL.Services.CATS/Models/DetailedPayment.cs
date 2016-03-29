using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Models
{
    public class DetailedPayment : CATSPaymentBatchItemDataModel
    {
        public BankAccountDataModel SourceBankAccount;
        public BankAccountDataModel TargetBankAccount;
        public DetailedPayment(CATSPaymentBatchItemDataModel parentModel, BankAccountDataModel sourceBankAccount, BankAccountDataModel targetBankAccount)
            : base(parentModel.GenericKey, parentModel.GenericTypeKey, parentModel.AccountKey, parentModel.Amount
            , parentModel.SourceBankAccountKey, parentModel.TargetBankAccountKey, parentModel.CATSPaymentBatchKey
            , parentModel.SahlReferenceNumber, parentModel.SourceReferenceNumber, parentModel.TargetName, parentModel.ExternalReference, parentModel.EmailAddress, parentModel.LegalEntityKey, parentModel.Processed)
        {
            this.SourceBankAccount = sourceBankAccount;
            this.TargetBankAccount = targetBankAccount;
        }
    }
}
