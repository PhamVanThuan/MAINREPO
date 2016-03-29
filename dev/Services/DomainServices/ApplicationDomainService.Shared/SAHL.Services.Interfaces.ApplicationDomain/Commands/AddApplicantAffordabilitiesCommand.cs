using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddApplicantAffordabilitiesCommand : ServiceCommand, IApplicationDomainCommand
    {
        [Required]
        public ApplicantAffordabilityModel ApplicantAffordabilityModel { get; protected set; }

        public AddApplicantAffordabilitiesCommand(ApplicantAffordabilityModel applicantAffordabilityModel)
        {
            this.ApplicantAffordabilityModel = applicantAffordabilityModel;
        }
    }
}
