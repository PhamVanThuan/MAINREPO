using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class DeleteUnconfirmedAffordabilityAssessmentCommand : ServiceCommand, IApplicationDomainCommand, IRequiresUnconfirmedAffordabilityAssessment
    {
        public DeleteUnconfirmedAffordabilityAssessmentCommand(int affordabilityAssessmentKey)
        {
            AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        [Required]
        public int AffordabilityAssessmentKey { get; protected set; }
    }
}