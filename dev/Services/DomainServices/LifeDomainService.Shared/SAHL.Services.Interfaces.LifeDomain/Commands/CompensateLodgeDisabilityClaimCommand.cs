using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class CompensateLodgeDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public CompensateLodgeDisabilityClaimCommand(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}