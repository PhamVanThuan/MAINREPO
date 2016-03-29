using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Search
{
    public interface ISearchServiceClient : IServiceHttpClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ISearchServiceCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : ISearchServiceQuery;
    }
}