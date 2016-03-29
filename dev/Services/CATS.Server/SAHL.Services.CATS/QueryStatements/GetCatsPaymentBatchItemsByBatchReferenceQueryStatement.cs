using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Queries;

namespace SAHL.Services.CATS.QueryStatements
{
    public class GetCatsPaymentBatchItemsByBatchReferenceQueryStatement : IServiceQuerySqlStatement<GetCatsPaymentBatchItemsByBatchReferenceQuery, CATSPaymentBatchItemDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT 
                         [CATSPaymentBatchItemKey]
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
                    FROM 
                        [2AM].[dbo].[CATSPaymentBatchItem] 
                    WHERE 
                        [CATSPaymentBatchKey] = @BatchKey";
        }
    }
}
