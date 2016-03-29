using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LifeDomain.Commands
{
    public class SendDisabilityClaimManualApprovalLetterCommand : ServiceCommand, ILifeDomainCommand
    {
        [Required]
        public int DisabilityClaimKey { get; protected set; }

        public SendDisabilityClaimManualApprovalLetterCommand(int disabilityClaimKey)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
        }
    }
}