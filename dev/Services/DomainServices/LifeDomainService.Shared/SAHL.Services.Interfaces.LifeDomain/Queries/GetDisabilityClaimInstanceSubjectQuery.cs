using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Queries
{
    public class GetDisabilityClaimInstanceSubjectQuery : ServiceQuery<string>, ILifeDomainQuery
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimInstanceSubjectQuery(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}