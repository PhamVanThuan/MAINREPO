using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetNewThirdPartyPaymentReferenceStatement : ISqlStatement<int>
    {
        public int BatchTypeKey { get; protected set; }

        public GetNewThirdPartyPaymentReferenceStatement(CATSPaymentBatchType batchType)
        {
            BatchTypeKey = Convert.ToInt32(batchType);
        }

        public string GetStatement()
        {
            return @"DECLARE @batchKey INT
                     EXEC [2AM].[dbo].[GetNextCATSPaymentBatchReference] @BatchTypeKey, @batchKey OUT
                     SELECT @batchKey";
        }
    }
}
