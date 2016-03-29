using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.CATS.Managers.Statements
{
    [InsertConventionExclude]
    public class InsertCATSPaymentBatchItemStatement : ISqlStatement<CATSPaymentBatchItemDataModel>
    {
        #region Properties

        public int LegalEntityKey { get; protected set; }

        public int GenericKey { get; protected set; }

        public int GenericTypeKey { get; protected set; }

        public int AccountKey { get; protected set; }

        public decimal Amount { get; protected set; }

        public int SourceBankAccountKey { get; protected set; }

        public int TargetBankAccountKey { get; protected set; }

        public int CATSPaymentBatchKey { get; protected set; }

        public string SahlReference { get; protected set; }

        public string SourceReferenceNumber { get; protected set; }

        public string TargetName { get; protected set; }

        public string ExternalReference { get; protected set; }

        public string EmailAddress { get; protected set; }

        public bool Processed { get; protected set; }

        #endregion

        public InsertCATSPaymentBatchItemStatement(CATSPaymentBatchItemModel thirdPartyInvoicePaymentBatchItemModel)
        {
            LegalEntityKey = thirdPartyInvoicePaymentBatchItemModel.LegalEntityKey;
            GenericKey = thirdPartyInvoicePaymentBatchItemModel.GenericKey;
            GenericTypeKey = thirdPartyInvoicePaymentBatchItemModel.GenericTypeKey;
            AccountKey = thirdPartyInvoicePaymentBatchItemModel.AccountKey;
            Amount = thirdPartyInvoicePaymentBatchItemModel.Amount;
            SourceBankAccountKey = thirdPartyInvoicePaymentBatchItemModel.SourceBankAccountKey;
            TargetBankAccountKey = thirdPartyInvoicePaymentBatchItemModel.TargetBankAccountKey;
            CATSPaymentBatchKey = thirdPartyInvoicePaymentBatchItemModel.CATSPaymentBatchKey;
            SahlReference = thirdPartyInvoicePaymentBatchItemModel.SahlReferenceNumber;
            SourceReferenceNumber = thirdPartyInvoicePaymentBatchItemModel.SourceReferenceNumber;
            TargetName = thirdPartyInvoicePaymentBatchItemModel.TargetName;
            ExternalReference = thirdPartyInvoicePaymentBatchItemModel.ExternalReference;
            EmailAddress = thirdPartyInvoicePaymentBatchItemModel.EmailAddress;
            Processed = thirdPartyInvoicePaymentBatchItemModel.Processed;
        }

        public string GetStatement()
        {
            return @"
                    INSERT INTO [2AM].[dbo].[CATSPaymentBatchItem]
                               ([GenericKey]
                               ,[GenericTypeKey]
                               ,[AccountKey]
                               ,[Amount]
                               ,[SourceBankAccountKey]
                               ,[TargetBankAccountKey]
                               ,[CATSPaymentBatchKey]
                               ,[SahlReferenceNumber]
                               ,[SourceReferenceNumber]
                               ,[LegalEntityKey]
                               ,[TargetName]
                               ,[ExternalReference]
                               ,[EmailAddress]
                               ,[Processed])
                         VALUES
                               (@GenericKey
                               ,@GenericTypeKey
                               ,@AccountKey
                               ,@Amount
                               ,@SourceBankAccountKey
                               ,@TargetBankAccountKey
                               ,@CATSPaymentBatchKey
                               ,@SahlReference
                               ,@SourceReferenceNumber
                               ,@LegalEntityKey
                               ,@TargetName
                               ,@ExternalReference
                               ,@EmailAddress
                               ,@Processed)";
        }
    }
}
