using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.AddressDomain
{
    public interface IAddressDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IAddressDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IAddressDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}