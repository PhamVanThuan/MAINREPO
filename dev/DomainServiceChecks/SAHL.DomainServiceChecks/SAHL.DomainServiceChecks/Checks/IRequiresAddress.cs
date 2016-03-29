using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresAddress : IDomainCommandCheck
    {
        int AddressKey { get; }
    }
}
