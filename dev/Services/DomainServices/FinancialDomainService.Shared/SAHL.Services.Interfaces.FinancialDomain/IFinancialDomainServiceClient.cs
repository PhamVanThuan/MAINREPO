using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.FinancialDomain
{
    public interface IFinancialDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFinancialDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IFinancialDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}