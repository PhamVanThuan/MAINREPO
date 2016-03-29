using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;


namespace SAHL.Services.Interfaces.MortgageLoanDomain
{
    public class MortgageLoanDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IMortgageLoanDomainServiceClient
    {
        public MortgageLoanDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IMortgageLoanDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IMortgageLoanDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
