
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;
namespace SAHL.Services.Interfaces.CATS.Queries
{
    public class GetNewThirdPartyPaymentBatchReferenceQuery : ServiceQuery<GetNewThirdPartyPaymentBatchReferenceQueryResult>, ICATSServiceQuery
    {
        public CATSPaymentBatchType BatchType { get; protected set; }

        public GetNewThirdPartyPaymentBatchReferenceQuery(CATSPaymentBatchType batchType)
        {
            BatchType = batchType;
        }
    }
}
