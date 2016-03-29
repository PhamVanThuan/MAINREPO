using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class LodgeDisabilityClaimCommand : ServiceCommand, ILifeDomainCommand, IDisabilityClaimLifeAccountModel
    {
        [Required]
        public int LifeAccountKey { get; protected set; }

        [Required]
        public int ClaimantLegalEntityKey { get; protected  set; }

        [Required]
        public Guid DisabilityClaimGuid { get; protected set; }

        public LodgeDisabilityClaimCommand(int lifeAccountKey, int claimantLegalEntityKey, Guid disabilityClaimGuid)
        {
            this.LifeAccountKey = lifeAccountKey;
            this.ClaimantLegalEntityKey = claimantLegalEntityKey;
            this.DisabilityClaimGuid = disabilityClaimGuid;
        }
    }
}