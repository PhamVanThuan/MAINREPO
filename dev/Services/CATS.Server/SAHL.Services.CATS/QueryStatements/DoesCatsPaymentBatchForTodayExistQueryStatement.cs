using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.Interfaces.CATS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.CATS.QueryStatements
{
    public class DoesCatsPaymentBatchForTodayExistQueryStatement : IServiceQuerySqlStatement<DoesCatsPaymentBatchForTodayExistQuery, DoesCatsPaymentBatchForTodayExistModel>
    {
        public string GetStatement()
        {
            var query = @"select (case when exists (
                            select *
                            from [2AM].[dbo].[CATSPaymentBatch]
                            where cast(CreatedDate as date) = cast(getdate() as date)
	                        and CATSPaymentBatchTypeKey = {0}
	                        and CATSPaymentBatchStatusKey in ({1}, {2})
                        )
                        then cast(1 as bit)
                        else cast(0 as bit) end)[BatchExists]";

            query = string.Format(query, (int)CATSPaymentBatchType.ThirdPartyInvoice,
                (int)CATSPaymentBatchStatus.Processed, (int)CATSPaymentBatchStatus.Processing);

            return query;
        }
    }
}
