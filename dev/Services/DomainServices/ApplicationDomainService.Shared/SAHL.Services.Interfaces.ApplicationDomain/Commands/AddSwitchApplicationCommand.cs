using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddSwitchApplicationCommand : ServiceCommand, IApplicationDomainCommand
    {
        [Required]
        public SwitchApplicationModel SwitchApplicationModel { get; protected set; }

        [Required]
        public Guid ApplicationId { get; protected set; }

        public AddSwitchApplicationCommand(SwitchApplicationModel switchApplicationModel, Guid applicationId)
        {
            this.SwitchApplicationModel = switchApplicationModel;
            this.ApplicationId = applicationId;
        }
    }
}