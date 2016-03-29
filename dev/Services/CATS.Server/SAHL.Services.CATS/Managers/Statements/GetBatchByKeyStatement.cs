using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetBatchByKeyStatement : ISqlStatement<CATSPaymentBatchDataModel>
    {
        public int BatchKey { get; protected set; }

        public GetBatchByKeyStatement(int batchKey)
        {
            BatchKey = batchKey;
        }

        public string GetStatement()
        {
            return @"SELECT 
                     [CATSPaymentBatchKey]
                    ,[CATSPaymentBatchTypeKey]
                    ,[CreatedDate]
                    ,[ProcessedDate]
                    ,[CATSPaymentBatchStatusKey]
                    ,[CATSFileSequenceNo]
                    ,[CATSFileName]
                FROM 
                    [2AM].[dbo].[CATSPaymentBatch]
                WHERE 
                    [CATSPaymentBatchKey] = @BatchKey";
        }
    }
}
