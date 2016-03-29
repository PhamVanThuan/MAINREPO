using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetLastProcessedBatchQueryStatement : ISqlStatement<CATSPaymentBatchDataModel>
    {
        public IEnumerable<int> ProcessedStates { get; protected set; }
        public int BatchTypeKey { get; protected set; }

        public GetLastProcessedBatchQueryStatement(CATSPaymentBatchType batchType)
        {
            this.BatchTypeKey = (int)batchType;
            this.ProcessedStates = new List<int> { (int)CATSPaymentBatchStatus.Processed, (int)CATSPaymentBatchStatus.Processing };
        }

        public string GetStatement()
        {
            return @"SELECT TOP 1 [CATSPaymentBatchKey]
                        ,[CATSPaymentBatchTypeKey]
                        ,[CreatedDate]
                        ,[ProcessedDate]
                        ,[CATSPaymentBatchStatusKey]
                        ,[CATSFileSequenceNo]
                        ,[CATSFileName]
                    FROM [2AM].[dbo].[CATSPaymentBatch]
                    WHERE 
                        CATSPaymentBatchTypeKey = @BatchTypeKey
                    AND CATSPaymentBatchStatusKey in @ProcessedStates
                    AND CATSFileName is not null
                    AND CATSFileName <> ''
                    ORDER BY 1 DESC";
        }
    }
}
