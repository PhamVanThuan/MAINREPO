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
    public class UserRequestStartActivityWithoutSplitCommandHandler : IServiceCommandHandler<UserRequestStartActivityWithoutSplitCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public UserRequestStartActivityWithoutSplitCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(UserRequestStartActivityWithoutSplitCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            var workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);
            var checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(instance, command.Activity.StateId);
            var result = commandHandler.CheckRuleCommand(checkActivityIsValidForStateCommand, metadata);
            var checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckActivityIsValidForStateCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            CheckInstanceIsNotLockedCommand checkInstanceIsNotLockedCommand = new CheckInstanceIsNotLockedCommand(instance.ID, command.Activity.ActivityID, command.Username);
            messages.Aggregate(commandHandler.HandleCommand(checkInstanceIsNotLockedCommand, metadata));
            if (!checkInstanceIsNotLockedCommand.Result)
            {
                return messages;
            }

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
                var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
                commandHandler.QueueUpCommandToBeProcessed(unlockInstanceCommand);

                return messages;
            }

            var saveContextualDataCommand = new SaveContextualDataCommand(contextualData, instance.ID);
            messages.Aggregate(commandHandler.HandleCommand(saveContextualDataCommand, metadata));

            var saveInstanceCommand = new SaveInstanceCommand(instance);
            messages.Aggregate(commandHandler.HandleCommand(saveInstanceCommand, metadata));

            //((IX2TLSProcess)process).Dispose();
            //((X2TLSMap)map).Dispose();
            return messages;
        }
    }
}