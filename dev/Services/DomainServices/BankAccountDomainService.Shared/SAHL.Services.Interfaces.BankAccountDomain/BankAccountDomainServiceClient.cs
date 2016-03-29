using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.BankAccountDomain
{
    public class BankAccountDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IBankAccountDomainServiceClient
    {
        public BankAccountDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IBankAccountDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IBankAccountDomainQuery
        {
            return base.PerformQueryInternal(query);
        }
    }
}