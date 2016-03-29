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
    public class HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler : IServiceCommandHandler<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = messageCollectionFactory.CreateEmptyCollection();
            DateTime startTime = DateTime.Now;
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            var workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);

            CheckActivityIsValidForStateCommand checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(instance, command.Activity.StateId);
            commandHandler.CheckRuleCommand<CheckActivityIsValidForStateCommand>(checkActivityIsValidForStateCommand, metadata);

            LockInstanceCommand lockInstanceCommand = new LockInstanceCommand(instance, command.UserName, command.Activity);
            messages.Aggregate(commandHandler.HandleCommand<LockInstanceCommand>(lockInstanceCommand, metadata));

            var process = processProvider.GetProcessForInstance(instance.ID);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));
            var contextualData = map.GetContextualData(instance.ID);

            contextualData.LoadData(instance.ID);

            DeleteAllScheduleActivitiesCommand deleteScheduledActivityCommand = new DeleteAllScheduleActivitiesCommand(command.InstanceId);
            messages.Aggregate(commandHandler.HandleCommand<DeleteAllScheduleActivitiesCommand>(deleteScheduledActivityCommand, metadata));
            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.StateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);

            var result = map.StartActivity(instance, contextualData, param, messages);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnStart);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                command.Result = handleMapReturnCommand.Result;
                return messages;
            }

            var activityMessage = map.GetActivityMessage(instance, contextualData, param, messages);
            if (string.IsNullOrEmpty(activityMessage))
                activityMessage = command.Activity.ActivityName;

            result = map.CompleteActivity(instance, contextualData, param, messages, ref activityMessage);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnComplete);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            result = map.ExitState(instance, contextualData, param, messages);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnExit);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            instance.StateID = command.Activity.ToStateId;
            instance.StateChangeDate = DateTime.Now;

            QueueUpExternalActivitiesCommand queueUpExternalActivitiesCommand = new QueueUpExternalActivitiesCommand(instance, command.Activity);
            messages.Aggregate(commandHandler.HandleCommand<QueueUpExternalActivitiesCommand>(queueUpExternalActivitiesCommand, metadata));
            param = new X2Params(command.Activity.ActivityName, command.Activity.ToStateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            result = map.EnterState(instance, contextualData, param, messages);
            handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnEnter);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            WakeUpSourceInstanceAndPerformReturnActivityCommand wakeUpSourceInstanceAndPerformReturnActivityCommand = new WakeUpSourceInstanceAndPerformReturnActivityCommand(instance, command.Activity, map, contextualData);
            commandHandler.HandleCommand<WakeUpSourceInstanceAndPerformReturnActivityCommand>(wakeUpSourceInstanceAndPerformReturnActivityCommand, metadata);

            SaveContextualDataCommand saveContextualDataCommand = new SaveContextualDataCommand(contextualData, instance.ID);
            commandHandler.HandleCommand<SaveContextualDataCommand>(saveContextualDataCommand, metadata);

            SaveInstanceCommand saveInstanceCommand = new SaveInstanceCommand(instance);
            commandHandler.HandleCommand<SaveInstanceCommand>(saveInstanceCommand, metadata);

            SaveWorkflowHistoryCommand saveWorkflowHistoryCommand = new SaveWorkflowHistoryCommand(instance, command.Activity.ToStateId, command.Activity.ActivityID, command.UserName, DateTime.Now);
            commandHandler.HandleCommand<SaveWorkflowHistoryCommand>(saveWorkflowHistoryCommand, metadata);

            var stageTransitionComment = map.GetStageTransition(instance, contextualData, param, messages);
            RecordStageTransitionCommand recordStageTransitionCommand = new RecordStageTransitionCommand(instance, stageTransitionComment, contextualData, command.UserName, command.Activity, startTime);
            commandHandler.HandleCommand<RecordStageTransitionCommand>(recordStageTransitionCommand, metadata);

            BuildSystemRequestToProcessCommand queueUpSystemRequestCommand = new BuildSystemRequestToProcessCommand(instance, contextualData);
            commandHandler.HandleCommand<BuildSystemRequestToProcessCommand>(queueUpSystemRequestCommand, metadata);

            RefreshWorklistCommand refreshWorklistCommand = new RefreshWorklistCommand(instance, contextualData, activityMessage, map);
            commandHandler.HandleCommand<RefreshWorklistCommand>(refreshWorklistCommand, metadata);

            RefreshInstanceActivitySecurityCommand refreshInstanceActivitySecurityCommand = new RefreshInstanceActivitySecurityCommand(instance, contextualData, map);
            commandHandler.HandleCommand<RefreshInstanceActivitySecurityCommand>(refreshInstanceActivitySecurityCommand, metadata);

            var unlockInstanceCommand = new UnlockInstanceCommand(instance.ID);
            messages.Aggregate(commandHandler.HandleCommand(unlockInstanceCommand, metadata));

            command.Result = true;
            return messages;
        }
    }
}