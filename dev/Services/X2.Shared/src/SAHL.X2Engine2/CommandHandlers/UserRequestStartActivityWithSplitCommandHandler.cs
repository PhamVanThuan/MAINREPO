using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UserRequestStartActivityWithSplitCommandHandler : IServiceCommandHandler<UserRequestStartActivityWithSplitCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public UserRequestStartActivityWithSplitCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(UserRequestStartActivityWithSplitCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);

            CheckInstanceIsNotLockedCommand checkInstanceIsNotLockedCommand = new CheckInstanceIsNotLockedCommand(instance.ID, command.Activity.ActivityID, command.Username);
            messages.Aggregate(commandHandler.HandleCommand(checkInstanceIsNotLockedCommand, metadata));
            if (!checkInstanceIsNotLockedCommand.Result)
            {
                return messages;
            }

            var checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(instance, command.Activity.StateId);
            var result = commandHandler.CheckRuleCommand(checkActivityIsValidForStateCommand, metadata);
            var checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckActivityIsValidForStateCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            var createInstanceCommand = new CreateChildInstanceCommand(instance, command.Activity, command.WorkflowProviderName, command.Username);
            messages.Aggregate(commandHandler.HandleCommand(createInstanceCommand, metadata));

            instance = createInstanceCommand.CreatedInstance;

            var workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);

            var lockInstanceCommand = new LockInstanceCommand(instance, command.Username, command.Activity);
            messages.Aggregate(commandHandler.HandleCommand(lockInstanceCommand, metadata));

            var process = processProvider.GetProcessForInstance(instance.ID);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));

            IX2ContextualDataProvider contextualData = map.GetContextualData(instance.ID);
            contextualData.LoadData(instance.ID);
            contextualData.SetMapVariables(command.MapVariables);
            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.StateName, workflow.Name, command.IgnoreWarnings, command.Username, metadata, command.Data);
            result = map.StartActivity(instance, contextualData, param, messages);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnStart);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                RemoveCloneInstanceCommand removeCloneInstanceCommand = new RemoveCloneInstanceCommand(createInstanceCommand.CreatedInstance.ID);
                commandHandler.QueueUpCommandToBeProcessed(removeCloneInstanceCommand);
                var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
                commandHandler.QueueUpCommandToBeProcessed(unlockInstanceCommand);
                return messages;
            }
            var saveContextualDataCommand = new SaveContextualDataCommand(contextualData, instance.ID);
            messages.Aggregate(commandHandler.HandleCommand(saveContextualDataCommand, metadata));

            var saveInstanceCommand = new SaveInstanceCommand(instance);
            messages.Aggregate(commandHandler.HandleCommand(saveInstanceCommand, metadata));

            return messages;
        }
    }
}