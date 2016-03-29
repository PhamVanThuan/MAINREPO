using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain
{   
    public interface IApplicationDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IApplicationDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IApplicationDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}