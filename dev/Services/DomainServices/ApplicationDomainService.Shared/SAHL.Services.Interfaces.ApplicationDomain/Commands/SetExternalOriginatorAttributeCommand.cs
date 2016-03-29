using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class SetExternalOriginatorAttributeCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid Application Number.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public OriginationSource OriginationSource { get; protected set; }

        public SetExternalOriginatorAttributeCommand(int applicationNumber, OriginationSource originationSource)
        {
            this.ApplicationNumber = applicationNumber;
            this.OriginationSource = originationSource;
        }
    }
}