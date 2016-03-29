using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class RepudiateDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        [Required]
        public IEnumerable<int> ReasonKeys { get; protected set; }

        public RepudiateDisabilityClaimCommand(int disabilityClaimKey, IEnumerable<int> reasonKeys)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.ReasonKeys = reasonKeys;
        }
    }
}