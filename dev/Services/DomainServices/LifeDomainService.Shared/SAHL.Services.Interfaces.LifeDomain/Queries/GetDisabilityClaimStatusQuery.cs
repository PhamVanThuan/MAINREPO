using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetDisabilityClaimStatusDescriptionQuery : ServiceQuery<string>, ILifeDomainQuery
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimStatusDescriptionQuery(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}