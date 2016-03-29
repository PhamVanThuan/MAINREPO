using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresAccount : IDomainCommandCheck
    {
        int AccountKey { get; }
    }
}