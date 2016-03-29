using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommandHandler : IServiceCommandHandler<HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            DateTime startTime = DateTime.Now;
            var parentInstance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            CheckActivityIsValidForStateCommand checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(parentInstance, command.Activity.StateId);
            var result = commandHandler.CheckRuleCommand<CheckActivityIsValidForStateCommand>(checkActivityIsValidForStateCommand, metadata);

            var checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckActivityIsValidForStateCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            DeleteScheduledActivityCommand deleteScheduledActivitySplitCommand = new DeleteScheduledActivityCommand(command.InstanceId, command.Activity.ActivityID);
            messages.Aggregate(commandHandler.HandleCommand<DeleteScheduledActivityCommand>(deleteScheduledActivitySplitCommand, metadata));

            CreateChildInstanceCommand createChildInstanceCommand = new CreateChildInstanceCommand(parentInstance, command.Activity, command.WorkflowProviderName, command.UserName);
            messages.Aggregate(commandHandler.HandleCommand<CreateChildInstanceCommand>(createChildInstanceCommand, metadata));

            var childInstance = workflowDataProvider.GetInstanceDataModel(createChildInstanceCommand.CreatedInstance.ID);

            var process = processProvider.GetProcessForInstance(parentInstance.ID);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(parentInstance));
            var childContextualData = map.GetContextualData(childInstance.ID);
            var parentContextualData = map.GetContextualData(parentInstance.ID);
            parentContextualData.LoadData(parentInstance.ID);
            var workflow = workflowDataProvider.GetWorkflowById(childInstance.WorkFlowID);

            CreateContextualDataCommand createContextualDataCommand = new CreateContextualDataCommand(childInstance, childContextualData, parentContextualData.GetData());
            messages.Aggregate(commandHandler.HandleCommand<CreateContextualDataCommand>(createContextualDataCommand, metadata));

            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.StateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            result = map.StartActivity(childInstance, childContextualData, param, messages);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnStart);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                RemoveCloneInstanceCommand removeCloneInstanceCommand = new RemoveCloneInstanceCommand(childInstance.ID);
                commandHandler.QueueUpCommandToBeProcessed(removeCloneInstanceCommand);
                var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
                commandHandler.QueueUpCommandToBeProcessed(unlockInstanceCommand);
                return messages;
            }

            var activityMessage = map.GetActivityMessage(childInstance, childContextualData, param, messages);
            if (string.IsNullOrEmpty(activityMessage))
                activityMessage = command.Activity.ActivityName;

            result = map.CompleteActivity(childInstance, childContextualData, param, messages, ref activityMessage);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnComplete);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            var stageTransitionComment = map.GetStageTransition(childInstance, childContextualData, param, messages);
            result = map.ExitState(childInstance, childContextualData, param, messages);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnExit);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);

            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }
            childInstance.StateID = command.Activity.ToStateId;
            childInstance.StateChangeDate = DateTime.Now;

            QueueUpExternalActivitiesCommand queueUpExternalActivitiesCommand = new QueueUpExternalActivitiesCommand(childInstance, command.Activity);
            messages.Aggregate(commandHandler.HandleCommand<QueueUpExternalActivitiesCommand>(queueUpExternalActivitiesCommand, metadata));
            param = new X2Params(command.Activity.ActivityName, command.Activity.ToStateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            result = map.EnterState(childInstance, childContextualData, param, messages);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnEnter);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            WakeUpSourceInstanceAndPerformReturnActivityCommand handleArchiveReturnCommand = new WakeUpSourceInstanceAndPerformReturnActivityCommand(childInstance, command.Activity, map, childContextualData);
            messages.Aggregate(commandHandler.HandleCommand<WakeUpSourceInstanceAndPerformReturnActivityCommand>(handleArchiveReturnCommand, metadata));

            SaveContextualDataCommand saveContextualDataCommand = new SaveContextualDataCommand(childContextualData, childInstance.ID);
            messages.Aggregate(commandHandler.HandleCommand<SaveContextualDataCommand>(saveContextualDataCommand, metadata));

            SaveInstanceCommand saveInstanceCommand = new SaveInstanceCommand(childInstance);
            messages.Aggregate(commandHandler.HandleCommand<SaveInstanceCommand>(saveInstanceCommand, metadata));

            SaveWorkflowHistoryCommand saveWorkflowHistoryCommand = new SaveWorkflowHistoryCommand(childInstance, command.Activity.ToStateId, command.Activity.ActivityID, command.UserName, DateTime.Now);
            messages.Aggregate(commandHandler.HandleCommand<SaveWorkflowHistoryCommand>(saveWorkflowHistoryCommand, metadata));

            var recordStageTransitionCommand = new RecordStageTransitionCommand(childInstance, stageTransitionComment, childContextualData, command.UserName, command.Activity, startTime);
            messages.Aggregate(commandHandler.HandleCommand<RecordStageTransitionCommand>(recordStageTransitionCommand, metadata));

            BuildSystemRequestToProcessCommand queueUpSystemRequestCommand = new BuildSystemRequestToProcessCommand(childInstance, childContextualData);
            messages.Aggregate(commandHandler.HandleCommand<BuildSystemRequestToProcessCommand>(queueUpSystemRequestCommand, metadata));

            RefreshWorklistCommand refreshWorklistCommand = new RefreshWorklistCommand(childInstance, childContextualData, activityMessage, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshWorklistCommand>(refreshWorklistCommand, metadata));

            RefreshInstanceActivitySecurityCommand refreshInstanceActivitySecurityCommand = new RefreshInstanceActivitySecurityCommand(childInstance, childContextualData, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshInstanceActivitySecurityCommand>(refreshInstanceActivitySecurityCommand, metadata));

            var unlockChildInstanceCommand = new UnlockInstanceCommand(childInstance.ID);
            messages.Aggregate(commandHandler.HandleCommand(unlockChildInstanceCommand, metadata));

            command.Result = true;
            return messages;
        }
    }
}