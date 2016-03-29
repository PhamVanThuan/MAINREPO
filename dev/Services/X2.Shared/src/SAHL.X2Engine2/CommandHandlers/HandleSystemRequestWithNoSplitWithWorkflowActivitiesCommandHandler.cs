using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommandHandler : IServiceCommandHandler<HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;

        public HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
        }

        public ISystemMessageCollection HandleCommand(HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand command, IServiceRequestMetadata metadata)
        {
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);

            CheckActivityIsValidForStateCommand checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(instance, instance.StateID);
            commandHandler.CheckRuleCommand<CheckActivityIsValidForStateCommand>(checkActivityIsValidForStateCommand, metadata);

            var process = processProvider.GetProcessForInstance(instance.ID);
            var sourceX2Map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));
            var destinationActivityCreate = workflowDataProvider.GetActivity(command.WorkFlowActivity.ActivityIDInDestinationMap);
            var destinationActivityViewModel = workflowDataProvider.GetActivityById(destinationActivityCreate.ID);

            UserRequestCreateInstanceCommand createInstanceCommand = new UserRequestCreateInstanceCommand(workflowDataProvider.GetProcessName(instance), command.WorkFlowActivity.DestinationWorkflowName,
                command.UserName, destinationActivityViewModel, null, command.WorkflowProviderName, command.IgnoreWarnings, null, command.InstanceId, command.WorkFlowActivity.ReturnActivityId, command.Data);
            commandHandler.HandleCommand<UserRequestCreateInstanceCommand>(createInstanceCommand, metadata);

            Activity activity = workflowDataProvider.GetActivityById(command.WorkFlowActivity.ActivityIDInDestinationMap);
            UserRequestCompleteCreateCommand userRequestCompleteCreateActivityCommand = new UserRequestCompleteCreateCommand(createInstanceCommand.NewlyCreatedInstanceId, activity, command.UserName, command.IgnoreWarnings, null, command.Data);
            commandHandler.HandleCommand<UserRequestCompleteCreateCommand>(userRequestCompleteCreateActivityCommand, metadata);

            command.Result = true;
            return new SystemMessageCollection();
        }
    }
}