using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class TerminateDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        [Required]
        public int ReasonKey { get; protected set; }

        public TerminateDisabilityClaimCommand(int disabilityClaimKey, int reasonKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.ReasonKey = reasonKey;
        }
    }
}