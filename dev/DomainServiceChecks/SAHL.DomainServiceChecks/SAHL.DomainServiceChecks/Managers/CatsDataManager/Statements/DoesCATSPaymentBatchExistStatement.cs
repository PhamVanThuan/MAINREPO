using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.CatsDataManager.Statements
{
    public class DoesCATSPaymentBatchExistStatement : ISqlStatement<int>
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public DoesCATSPaymentBatchExistStatement(int catsPaymentBatchKey)
        {
            this.CATSPaymentBatchKey = catsPaymentBatchKey;
        }

        public string GetStatement()
        {
            return @"SELECT count(*)
                      FROM [2AM].[dbo].[CATSPaymentBatch]
                      where [CATSPaymentBatchKey] = @CATSPaymentBatchKey";
        }
    }
}
