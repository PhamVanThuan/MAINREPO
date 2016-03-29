using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.EventProjectionStreamer
{
    public interface IEventProjectionStreamerServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IEventProjectionStreamerCommand;
    }
}