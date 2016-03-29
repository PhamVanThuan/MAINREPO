using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetPendingDisabilityClaimByAccountQuery : ServiceQuery<DisabilityClaimModel>, ILifeDomainQuery
    {
        [Required]
        public int AccountKey { get; protected set; }

        public GetPendingDisabilityClaimByAccountQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}