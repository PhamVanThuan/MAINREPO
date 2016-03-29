using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.CATS.Queries
{
    public class GetCatsPaymentBatchItemsByBatchReferenceQuery : ServiceQuery<CATSPaymentBatchItemDataModel>, ICATSServiceQuery, ISqlServiceQuery<CATSPaymentBatchItemDataModel>
    {
        public int BatchKey { get; protected set; }

        public GetCatsPaymentBatchItemsByBatchReferenceQuery(int batchKey)
        {
            BatchKey = batchKey;
        }
    }
}
