using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.FinanceDomain
{
    public interface IFinanceDomainServiceClient : IServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFinanceDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IFinanceDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
