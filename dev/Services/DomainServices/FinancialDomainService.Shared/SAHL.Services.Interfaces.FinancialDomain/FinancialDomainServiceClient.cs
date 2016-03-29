using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.FinancialDomain
{
    public class FinancialDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IFinancialDomainServiceClient
    {
        public FinancialDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFinancialDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T :IFinancialDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}