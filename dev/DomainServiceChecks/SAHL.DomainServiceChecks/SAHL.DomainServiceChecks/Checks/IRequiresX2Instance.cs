using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresX2Instance : IDomainCommandCheck
    {
        int InstanceId { get; }
    }
}