using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.DomainProcessManagerProxy
{
    public interface IDomainProcessManagerProxyServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IDomainProcessManagerProxyCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IDomainProcessManagerProxyQuery;
    }
}
