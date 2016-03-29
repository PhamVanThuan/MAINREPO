using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.Search
{
    public class SearchServiceClient : ServiceHttpClientWindowsAuthenticated, ISearchServiceClient
    {
        public SearchServiceClient(IServiceUrlConfigurationProvider urlConfiguration, IJsonActivator jsonActivator)
            : base(urlConfiguration, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ISearchServiceCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : ISearchServiceQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}