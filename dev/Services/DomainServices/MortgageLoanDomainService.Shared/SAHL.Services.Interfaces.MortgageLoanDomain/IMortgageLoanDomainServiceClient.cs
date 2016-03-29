using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.MortgageLoanDomain
{
    public interface IMortgageLoanDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IMortgageLoanDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IMortgageLoanDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
