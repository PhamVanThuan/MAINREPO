using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.PersonalLoanDomain
{
    public interface IPersonalLoanDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IPersonalLoanDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IPersonalLoanDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
