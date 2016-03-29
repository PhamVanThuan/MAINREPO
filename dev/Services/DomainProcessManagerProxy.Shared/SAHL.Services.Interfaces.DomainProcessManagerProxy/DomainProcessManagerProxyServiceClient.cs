using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.DomainProcessManagerProxy
{
    public class DomainProcessManagerProxyServiceClient : ServiceHttpClientWindowsAuthenticated, IDomainProcessManagerProxyServiceClient
    {
        public DomainProcessManagerProxyServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, Core.Services.IServiceRequestMetadata metadata) where T : IDomainProcessManagerProxyCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IDomainProcessManagerProxyQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
