using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;


namespace SAHL.Services.Interfaces.FinanceDomain
{
    public class FinanceDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IFinanceDomainServiceClient
    {
        public FinanceDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFinanceDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IFinanceDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
