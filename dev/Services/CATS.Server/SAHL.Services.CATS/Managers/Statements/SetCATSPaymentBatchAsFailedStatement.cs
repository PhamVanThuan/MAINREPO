using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class SetCATSPaymentBatchAsFailedStatement : ISqlStatement<object>
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public SetCATSPaymentBatchAsFailedStatement(int cATSPaymentBatchKey)
        {
            this.CATSPaymentBatchKey = cATSPaymentBatchKey;
        }

        public string GetStatement()
        {
            var failedStatusKey = (int)CATSPaymentBatchStatus.Failed;
            var query = @"
                            update 
	                            CATSPaymentBatch
                            set 
	                            CATSPaymentBatchStatusKey = {0}
                            where
	                            CATSPaymentBatchKey = {1}";

            query = string.Format(query, failedStatusKey, CATSPaymentBatchKey);

            return query;
        }
    }
}
