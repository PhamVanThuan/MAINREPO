using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.PropertyDomain
{
    public interface IPropertyDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IPropertyDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IPropertyDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
