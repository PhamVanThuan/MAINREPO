using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class QueueUpExternalActivitiesCommandHandler : IServiceCommandHandler<QueueUpExternalActivitiesCommand>
    {
        private IX2NodeConfigurationProvider nodeConfigurationProvider;
        private IX2RequestInterrogator interrogator;
        private IX2RequestPublisher requestPublisher;
        private IX2QueueNameBuilder x2QueueNameBuilder;

        public QueueUpExternalActivitiesCommandHandler(IX2NodeConfigurationProvider nodeConfigurationProvider, IX2RequestPublisher requestPublisher, IX2RequestInterrogator interrogator, IX2QueueNameBuilder x2QueueNameBuilder)
        {
            this.nodeConfigurationProvider = nodeConfigurationProvider;
            this.requestPublisher = requestPublisher;
            this.interrogator = interrogator;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
        }

        public ISystemMessageCollection HandleCommand(QueueUpExternalActivitiesCommand command, IServiceRequestMetadata metadata)
        {
            if (command.Activity.RaiseExternalActivityId == null)
                return new SystemMessageCollection();

            X2ExternalActivityRequest request = new X2ExternalActivityRequest(command.Id, command.Instance.ID, 0, (int)command.Activity.RaiseExternalActivityId, command.Instance.ID, command.Instance.WorkFlowID, metadata, null);
            var workflow = interrogator.GetRequestWorkflow(request);
            var systemQueue = this.x2QueueNameBuilder.GetSystemQueue(workflow);
            requestPublisher.Publish<X2ExternalActivityRequest>(systemQueue, request);
            return new SystemMessageCollection();
        }
    }
}