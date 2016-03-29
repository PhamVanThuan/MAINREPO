using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresUnconfirmedAffordabilityAssessment : IDomainCommandCheck
    {
        int AffordabilityAssessmentKey { get; }
    }
}