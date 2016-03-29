using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;


namespace SAHL.Services.Interfaces.ThirdPartyDomain
{
    public class ThirdPartyDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IThirdPartyDomainServiceClient
    {
        public ThirdPartyDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IThirdPartyDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IThirdPartyDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
