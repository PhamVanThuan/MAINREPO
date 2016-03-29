using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class SetApplicationEmploymentTypeCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication 
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid Application Number.")]
        public int ApplicationNumber { get; protected set; }

        public SetApplicationEmploymentTypeCommand(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }
    }
}