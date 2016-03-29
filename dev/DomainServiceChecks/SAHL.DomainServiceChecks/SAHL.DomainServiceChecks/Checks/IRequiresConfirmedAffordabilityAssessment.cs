using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresConfirmedAffordabilityAssessment : IDomainCommandCheck
    {
        int AffordabilityAssessmentKey { get; }
    }
}