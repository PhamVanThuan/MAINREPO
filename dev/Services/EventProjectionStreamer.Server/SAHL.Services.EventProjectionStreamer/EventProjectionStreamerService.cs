using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Services.EventProjectionStreamer
{
    public class EventProjectionStreamerService : IEventProjectionStreamerServiceClient
    {
        private readonly IServiceCommandRouter _serviceCommandRouter;

        public EventProjectionStreamerService(IServiceCommandRouter serviceCommandRouter)
        {
            if (serviceCommandRouter == null) { throw new ArgumentNullException("serviceCommandRouter"); }
            _serviceCommandRouter = serviceCommandRouter;
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IEventProjectionStreamerCommand
        {
            return this._serviceCommandRouter.HandleCommand<T>(command, metadata);
        }
    }
}