using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class RebuildInstanceCommandHandler : IServiceCommandHandler<RebuildInstanceCommand>
    {
        IX2ProcessProvider processProvider;
        IX2ServiceCommandRouter commandHandler;
        IWorkflowDataProvider workflowDataProvider;

        public RebuildInstanceCommandHandler(IX2ServiceCommandRouter commandHandler, IX2ProcessProvider procesProvider, IWorkflowDataProvider workflowDataProvider)
        {
            this.commandHandler = commandHandler;
            this.processProvider = procesProvider;
            this.workflowDataProvider = workflowDataProvider;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(RebuildInstanceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            var process = processProvider.GetProcessForInstance(command.InstanceId);
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));

            var contextualData = map.GetContextualData(instance.ID);
            contextualData.LoadData(instance.ID);

            string activityMessage = string.Empty;
            RefreshWorklistCommand refreshWorklistCommand = new RefreshWorklistCommand(instance, contextualData, activityMessage, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshWorklistCommand>(refreshWorklistCommand, metadata));

            RefreshInstanceActivitySecurityCommand refreshInstanceActivitySecurityCommand = new RefreshInstanceActivitySecurityCommand(instance, contextualData, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshInstanceActivitySecurityCommand>(refreshInstanceActivitySecurityCommand, metadata));

            return messages;
        }
    }
}