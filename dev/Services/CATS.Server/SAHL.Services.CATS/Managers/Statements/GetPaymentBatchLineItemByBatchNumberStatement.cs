using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetPaymentBatchLineItemsByBatchKeyStatement : ISqlStatement<CATSPaymentBatchItemDataModel>
    {
        public int PaymentBatchKey { get; protected set; }
        public GetPaymentBatchLineItemsByBatchKeyStatement(int paymentBatchKey)
        {
            PaymentBatchKey = paymentBatchKey;
        }
        public string GetStatement()
        {
            return @"SELECT [CATSPaymentBatchItemKey]
                      ,[GenericKey]
                      ,[GenericTypeKey]
                      ,[AccountKey]
                      ,[Amount]
                      ,[SourceBankAccountKey]
                      ,[TargetBankAccountKey]
                      ,[CATSPaymentBatchKey]
                      ,[SahlReferenceNumber]
                      ,[SourceReferenceNumber]
                      ,[TargetName]
                      ,[ExternalReference]
                      ,[EmailAddress]
                      ,[LegalEntityKey]
                      ,[Processed]
                  FROM [2AM].[dbo].[CATSPaymentBatchItem]
                  Where CATSPaymentBatchKey = @PaymentBatchKey
                        AND Processed = 1";
        }
    }
}
