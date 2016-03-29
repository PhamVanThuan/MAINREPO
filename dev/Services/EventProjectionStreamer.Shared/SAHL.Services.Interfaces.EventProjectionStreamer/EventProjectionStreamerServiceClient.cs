using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.EventProjectionStreamer
{
    public class EventProjectionStreamerServiceClient : ServiceHttpClientWindowsAuthenticated, IEventProjectionStreamerServiceClient
    {
        public EventProjectionStreamerServiceClient(IServiceUrlConfigurationProvider serviceConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata serviceRequestMetaData) where T : IEventProjectionStreamerCommand
        {
            return base.PerformCommandInternal<T>(command, serviceRequestMetaData);
        }
    }
}