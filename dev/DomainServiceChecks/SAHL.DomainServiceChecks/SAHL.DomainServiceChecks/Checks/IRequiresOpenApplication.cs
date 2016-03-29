
using SAHL.Core.Services;
namespace SAHL.DomainServiceChecks.Checks
{
    public interface IRequiresOpenApplication : IDomainCommandCheck
    {
        int ApplicationNumber { get; }
    }
}
