using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.DomainQuery
{
    public class DomainQueryService : ServiceHttpClient, IDomainQueryServiceClient
    {
        public DomainQueryService(IServiceUrlConfigurationProvider serviceUrlConfiguration, IJsonActivator jsonActivator)
           : base(serviceUrlConfiguration, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IDomainQueryQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}