using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.ClientDomain
{
    public class ClientDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IClientDomainServiceClient
    {
        public ClientDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IClientDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IClientDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
