using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Communications
{
    public interface ICommunicationsServiceClient : IServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ICommunicationsServiceCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : ICommunicationsQuery;        
    }
}