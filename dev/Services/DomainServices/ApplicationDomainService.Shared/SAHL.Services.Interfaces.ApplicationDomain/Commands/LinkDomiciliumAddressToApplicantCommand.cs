using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class LinkDomiciliumAddressToApplicantCommand : ServiceCommand, IApplicationDomainCommand, IRequiresClient, IRequiresOpenApplication
    {
        [Required]
        public ApplicantDomiciliumAddressModel ApplicantDomicilium { get; protected set; }

        [Required]
        public string AdUserName { get; protected set; }

        public int ApplicationNumber { get { return ApplicantDomicilium.ApplicationNumber; } }

        public int ClientKey { get { return ApplicantDomicilium.ClientKey; } }

        public LinkDomiciliumAddressToApplicantCommand(ApplicantDomiciliumAddressModel applicantDomicilium)
        {
            this.ApplicantDomicilium = applicantDomicilium;
            this.AdUserName = "System";
        }
    }
}