using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddRefinanceApplicationCommand : ServiceCommand, IApplicationDomainCommand
    {
        [Required]
        public RefinanceApplicationModel RefinanceApplicationModel { get; protected set; }

        [Required]
        public Guid ApplicationId { get; protected set; }

        public AddRefinanceApplicationCommand(RefinanceApplicationModel refinanceApplicationModel, Guid applicationId)
        {
            this.RefinanceApplicationModel = refinanceApplicationModel;
            this.ApplicationId = applicationId;
        }
    }
}