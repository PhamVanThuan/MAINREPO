using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;


namespace SAHL.Services.Interfaces.PersonalLoanDomain
{
    public class PersonalLoanDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IPersonalLoanDomainServiceClient
    {
        public PersonalLoanDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IPersonalLoanDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IPersonalLoanDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
