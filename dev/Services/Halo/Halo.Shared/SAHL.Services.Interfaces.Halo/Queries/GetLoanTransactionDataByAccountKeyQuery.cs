using SAHL.Core.Services;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo.Queries
{
    public class GetLoanTransactionDataByAccountKeyQuery : ServiceQuery<LoanTransactionDetailModel>, IHaloServiceQuery
    {
        public int AccountKey
        { get; protected set; }

        public GetLoanTransactionDataByAccountKeyQuery(int businessKey)
        {
            this.AccountKey = businessKey;
        }
    }
}