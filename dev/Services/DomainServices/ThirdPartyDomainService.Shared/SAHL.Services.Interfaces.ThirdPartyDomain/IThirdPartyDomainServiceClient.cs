using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.ThirdPartyDomain
{
    public interface IThirdPartyDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IThirdPartyDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IThirdPartyDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
