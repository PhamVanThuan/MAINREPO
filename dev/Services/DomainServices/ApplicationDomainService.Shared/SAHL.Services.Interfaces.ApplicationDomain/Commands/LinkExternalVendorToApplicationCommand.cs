using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class LinkExternalVendorToApplicationCommand : ServiceCommand, IApplicationDomainCommand, IRequiresOpenApplication
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid Application Number.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public OriginationSource OriginationSource { get; protected set; }

        [Required]
        public string VendorCode { get; protected set; }

        public LinkExternalVendorToApplicationCommand(int applicationNumber, OriginationSource originationSource, string vendorCode)
        {
            this.ApplicationNumber = applicationNumber;
            this.VendorCode = vendorCode;
            this.OriginationSource = originationSource;           
        }
    }
}
