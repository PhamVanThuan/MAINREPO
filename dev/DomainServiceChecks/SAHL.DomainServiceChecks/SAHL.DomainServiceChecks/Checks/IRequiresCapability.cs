using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresCapability : IDomainCommandCheck
    {
        int CapabilityKey { get; }
    }
}