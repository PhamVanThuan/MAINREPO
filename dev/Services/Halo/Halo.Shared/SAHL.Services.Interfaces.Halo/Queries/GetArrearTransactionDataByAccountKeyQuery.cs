using SAHL.Core.Services;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo.Queries
{
    public class GetArrearTransactionDataByAccountKeyQuery : ServiceQuery<ArrearTransactionDetailModel>, IHaloServiceQuery
    {
        public GetArrearTransactionDataByAccountKeyQuery(int businessKey)
        {
            this.AccountKey = businessKey;
        }

        public int AccountKey { get; protected set; }
    }
}