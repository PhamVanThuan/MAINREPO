using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class ConfirmApplicationAffordabilityAssessmentsCommand : ServiceCommand, IApplicationDomainCommand
    {
        public ConfirmApplicationAffordabilityAssessmentsCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; protected set; }
    }
}