using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class NotificationOfNewFutureScheduledActivityCommandHandler : IServiceCommandHandler<NotificationOfNewFutureScheduledActivityCommand>
    {
        private IX2NodeConfigurationProvider nodeConfigurationProvider;
        private IX2RequestPublisher requestPublisher;
        private IX2RequestInterrogator interrogator;

        public NotificationOfNewFutureScheduledActivityCommandHandler(IX2NodeConfigurationProvider nodeConfigurationProvider, IX2RequestPublisher requestPublisher, IX2RequestInterrogator interrogator)
        {
            this.nodeConfigurationProvider = nodeConfigurationProvider;
            this.requestPublisher = requestPublisher;
            this.interrogator = interrogator;
        }

        public ISystemMessageCollection HandleCommand(NotificationOfNewFutureScheduledActivityCommand command, IServiceRequestMetadata metadata)
        {
            X2NotificationOfNewScheduledActivityRequest request = new X2NotificationOfNewScheduledActivityRequest(command.InstanceId, command.ActivityId);
            var workflow = interrogator.GetRequestWorkflow(request);
            var route = new X2RouteEndpoint(X2QueueManager.X2EngineTimersRefreshExchange,X2QueueManager.X2EngineTimersRefreshQueue);
            requestPublisher.Publish<X2NotificationOfNewScheduledActivityRequest>(route, request);
            return new SystemMessageCollection();

        }
    }
}
