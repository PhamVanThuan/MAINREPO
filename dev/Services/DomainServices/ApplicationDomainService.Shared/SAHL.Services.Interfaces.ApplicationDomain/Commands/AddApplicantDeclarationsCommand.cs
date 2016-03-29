using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AddApplicantDeclarationsCommand : ServiceCommand, IApplicationDomainCommand, IRequiresClient, IRequiresOpenApplication
    {
        [Required]
        public ApplicantDeclarationsModel ApplicantDeclarations { get; protected set; }

        public int ApplicationNumber { get { return ApplicantDeclarations.ApplicationNumber; } }

        public int ClientKey { get { return ApplicantDeclarations.ClientKey; } }

        public AddApplicantDeclarationsCommand(ApplicantDeclarationsModel applicantDeclarations)
        {
            this.ApplicantDeclarations = applicantDeclarations;
        }
    }
}