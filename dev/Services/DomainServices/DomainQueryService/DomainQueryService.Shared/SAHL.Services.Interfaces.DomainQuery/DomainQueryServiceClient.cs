using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.DomainQuery
{
    public class DomainQueryServiceClient : ServiceHttpClientWindowsAuthenticated, IDomainQueryServiceClient
    {
        public DomainQueryServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        { }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IDomainQueryQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}