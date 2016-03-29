using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.Interfaces.CATS.Queries
{
    public class GetPreviousFileFailureQuery : ServiceQuery<GetPreviousFileFailureQueryResult>, ICATSServiceQuery
    {
        public CATSPaymentBatchType CATSPaymentBatchType { get; protected set; }

        public GetPreviousFileFailureQuery(CATSPaymentBatchType catsPaymentBatchType)
        {
            CATSPaymentBatchType = catsPaymentBatchType;
        }
    }
}
