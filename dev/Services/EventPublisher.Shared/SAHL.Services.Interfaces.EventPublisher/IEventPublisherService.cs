using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.EventPublisher
{
    public interface IEventPublisherService
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IEventPublisherCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IEventPublisherQuery;
    }
}