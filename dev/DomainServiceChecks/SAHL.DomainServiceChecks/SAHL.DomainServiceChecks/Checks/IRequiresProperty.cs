using SAHL.Core.Services;

namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresProperty : IDomainCommandCheck
    {
        int PropertyKey { get; }
    }
}
