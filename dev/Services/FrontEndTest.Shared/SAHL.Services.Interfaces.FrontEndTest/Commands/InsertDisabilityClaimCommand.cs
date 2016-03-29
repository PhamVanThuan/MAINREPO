using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertDisabilityClaimCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public DisabilityClaimDataModel model { get; protected set; }
        public Guid disabilityClaimGuid { get; protected set; }

        public InsertDisabilityClaimCommand(DisabilityClaimDataModel model, Guid disabilityClaimGuid)
        {
            this.model = model;
            this.disabilityClaimGuid = disabilityClaimGuid;
        }
    }
}