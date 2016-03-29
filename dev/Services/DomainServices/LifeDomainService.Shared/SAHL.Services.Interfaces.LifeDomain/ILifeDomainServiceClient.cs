using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.LifeDomain
{
    public interface ILifeDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ILifeDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : ILifeDomainQuery;

        event ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}