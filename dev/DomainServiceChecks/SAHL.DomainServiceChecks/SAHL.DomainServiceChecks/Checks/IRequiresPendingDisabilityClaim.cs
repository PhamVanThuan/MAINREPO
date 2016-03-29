using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresPendingDisabilityClaim : IDomainCommandCheck
    {
        int DisabilityClaimKey { get; }
    }
}