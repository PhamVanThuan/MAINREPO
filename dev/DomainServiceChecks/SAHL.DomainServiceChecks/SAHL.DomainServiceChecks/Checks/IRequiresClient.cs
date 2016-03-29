using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresClient : IDomainCommandCheck
    {
        int ClientKey { get; }
    }
}
