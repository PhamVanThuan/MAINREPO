using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.ApplicationDomain
{
    public class ApplicationDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IApplicationDomainServiceClient
    {
        public ApplicationDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IApplicationDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IApplicationDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}