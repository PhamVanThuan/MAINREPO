using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetPaymentBatchTypeByPaymentBatchKeyStatement : ISqlStatement<CATSPaymentBatchTypeDataModel>
    {
        public int PaymentBatchKey { get; protected set; }
        public GetPaymentBatchTypeByPaymentBatchKeyStatement(int batchKey)
        {
            this.PaymentBatchKey = batchKey;
        }
        public string GetStatement()
        {
            return @"SELECT [CATSPaymentBatchTypeKey]
                            ,[Description]
                            ,[CATSProfile]
                            ,[CATSFileNamePrefix]
                            ,[CATSEnvironment]
                            ,[NextCATSFileSequenceNo]
                  FROM [2AM].[dbo].[CATSPaymentBatchType]
                  WHERE [CATSPaymentBatchTypeKey] = (SELECT TOP 1 [CATSPaymentBatchTypeKey]
                                                      FROM [2AM].[dbo].[CATSPaymentBatch]
                                                      WHERE [CATSPaymentBatchKey] = @PaymentBatchKey)";
        }
    }
}
