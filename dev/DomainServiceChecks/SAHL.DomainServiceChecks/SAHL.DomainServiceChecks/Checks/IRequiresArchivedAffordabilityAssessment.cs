using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresArchivedAffordabilityAssessment : IDomainCommandCheck
    {
        int AffordabilityAssessmentKey { get; }
    }
}