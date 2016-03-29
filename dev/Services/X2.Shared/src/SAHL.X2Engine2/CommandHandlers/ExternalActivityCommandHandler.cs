using System;
using SAHL.Core.Services;
using SAHL.Core.X2.Factories;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class ExternalActivityCommandHandler : IServiceCommandHandler<ExternalActivityCommand>
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ServiceCommandRouter commandHandler;
        private IMessageCollectionFactory messageCollectionFactory;
        private string workflowDataProviderName;

        public ExternalActivityCommandHandler(IWorkflowDataProvider workflowDataProvider, IX2ServiceCommandRouter commandHandler, IMessageCollectionFactory messageCollectionFactory, IX2NodeConfigurationProvider nodeConfigurationProvider)
        {
            this.workflowDataProviderName = nodeConfigurationProvider.GetExchangeName();
            this.workflowDataProvider = workflowDataProvider;
            this.commandHandler = commandHandler;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ExternalActivityCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = messageCollectionFactory.CreateEmptyCollection();

            var activatedExternalActivities = workflowDataProvider.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(command.ExternalActivityID, command.ActivatingInstanceID);
            foreach (var activatedExternalActivity in activatedExternalActivities)
            {
                var instanceDataModel = workflowDataProvider.GetInstanceDataModel(activatedExternalActivity.InstanceId ?? -1);
                var activityDataModel = workflowDataProvider.GetActivityByActivatingExternalActivity(command.ExternalActivityID, instanceDataModel == null ? -1 : (int)instanceDataModel.StateID);
                var activity = workflowDataProvider.GetActivityById(activityDataModel.ID);

                if (activity.StateId == null || activatedExternalActivity.ExternalActivityTarget == 3)//create
                {
                    var workflowDataModel = workflowDataProvider.GetWorkflowById(activityDataModel.WorkFlowID);
                    var processDataModel = workflowDataProvider.GetProcessById(workflowDataModel.ProcessID);

                    UserRequestCreateInstanceCommand userCreateCommand = new UserRequestCreateInstanceCommand(processDataModel.Name, workflowDataModel.Name, "X2", activity, command.MapVariables, workflowDataProviderName, command.IgnoreWarnings);
                    systemMessages.Aggregate(commandHandler.HandleCommand(userCreateCommand, metadata));
                    if (userCreateCommand.NewlyCreatedInstanceId <= 0)
                    {
                        throw new Exception(string.Format("Unable to create instance for external activity:{0}", command.ExternalActivityID));
                    }
                    UserRequestCompleteCreateCommand userCompleteCreateCommand = new UserRequestCompleteCreateCommand(userCreateCommand.NewlyCreatedInstanceId, activity, "X2", command.IgnoreWarnings, command.MapVariables, command.Data);
                    systemMessages.Aggregate(commandHandler.HandleCommand(userCompleteCreateCommand, metadata));
                }
                else
                {
                    if (activity.SplitWorkflow)
                    {
                        UserRequestStartActivityWithSplitCommand userRequestStartActivityWithoutSplitCommand = new UserRequestStartActivityWithSplitCommand(
                            (long)activatedExternalActivity.InstanceId, "X2", activity, command.MapVariables, workflowDataProviderName, command.IgnoreWarnings, command.Data);
                        systemMessages.Aggregate(commandHandler.HandleCommand(userRequestStartActivityWithoutSplitCommand, metadata));
                    }
                    else
                    {
                        UserRequestStartActivityWithoutSplitCommand userRequestStartActivityWithoutSplitCommand = new UserRequestStartActivityWithoutSplitCommand(
                            (long)activatedExternalActivity.InstanceId, "X2", activity, command.IgnoreWarnings, command.MapVariables, command.Data);
                        systemMessages.Aggregate(commandHandler.HandleCommand(userRequestStartActivityWithoutSplitCommand, metadata));
                    }
                    UserRequestCompleteActivityCommand userRequestCompleteActivityCommand =
                        new UserRequestCompleteActivityCommand((long)activatedExternalActivity.InstanceId, activity, "X2", command.IgnoreWarnings, command.MapVariables, command.Data);
                    systemMessages.Aggregate(commandHandler.HandleCommand(userRequestCompleteActivityCommand, metadata));
                }
            }

            return systemMessages;
        }
    }
}