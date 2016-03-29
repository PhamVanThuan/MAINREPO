using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class RemoveCATSPaymentBatchItemStatement : ISqlStatement<CATSPaymentBatchItemDataModel>
    {
        public int GenericKey { get; protected set; }
        public int GenericTypeKey { get; protected set; }
        public int CATSPaymentBatchKey { get; protected set; }

        public RemoveCATSPaymentBatchItemStatement(int catsPaymentBatchKey, int genericKey, int genericKeyTypeKey)
        {
            GenericKey = genericKey;
            GenericTypeKey = genericKeyTypeKey;
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }

        public string GetStatement()
        {
            return @"UPDATE [2AM].[dbo].[CATSPaymentBatchItem]
                        SET
                            Processed = 0
                        WHERE 
	                        GenericKey = @GenericKey
                        AND CATSPaymentBatchKey = @CATSPaymentBatchKey
                        AND GenericTypeKey = @GenericTypeKey";
        }
    }
}