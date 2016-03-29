using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using System;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UserRequestCompleteCreateCommandHandler : IServiceCommandHandler<UserRequestCompleteCreateCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        private IMessageCollectionFactory messageCollectionFactory;

        public UserRequestCompleteCreateCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(UserRequestCompleteCreateCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            DateTime startTime = DateTime.Now;
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            var workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);

            CheckInstanceIsLockedForUserCommand checkInstanceIsLockedForUserCommand = new CheckInstanceIsLockedForUserCommand(command.InstanceId, command.Activity.ActivityID, command.UserName);
            var result = commandHandler.CheckRuleCommand<CheckInstanceIsLockedForUserCommand>(checkInstanceIsLockedForUserCommand, metadata);
            var checkRuleResultCommand = new CheckRuleResultCommand(result, "CheckInstanceIsLockedForUserCommand");
            commandHandler.HandleCommand(checkRuleResultCommand, metadata);

            var process = processProvider.GetProcessForInstance(instance.ID);
            var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(instance));

            var contextualData = map.GetContextualData(instance.ID);
            contextualData.LoadData(instance.ID);
            contextualData.SetMapVariables(command.MapVariables);
            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.StateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            var activityMessage = map.GetActivityMessage(instance, contextualData, param, messages);
            if (string.IsNullOrEmpty(activityMessage))
                activityMessage = command.Activity.ActivityName;

            result = map.CompleteActivity(instance, contextualData, param, messages, ref activityMessage);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnComplete);
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

            SaveContextualDataCommand saveContextualDataCommand = new SaveContextualDataCommand(contextualData, instance.ID);
            messages.Aggregate(commandHandler.HandleCommand<SaveContextualDataCommand>(saveContextualDataCommand, metadata));

            SaveInstanceCommand saveInstanceCommand = new SaveInstanceCommand(instance);
            messages.Aggregate(commandHandler.HandleCommand<SaveInstanceCommand>(saveInstanceCommand, metadata));

            SaveWorkflowHistoryCommand saveWorkflowHistoryCommand = new SaveWorkflowHistoryCommand(instance, command.Activity.ToStateId, command.Activity.ActivityID, command.UserName, DateTime.Now);
            messages.Aggregate(commandHandler.HandleCommand<SaveWorkflowHistoryCommand>(saveWorkflowHistoryCommand, metadata));

            BuildSystemRequestToProcessCommand queueUpSystemRequestCommand = new BuildSystemRequestToProcessCommand(instance, contextualData);
            messages.Aggregate(commandHandler.HandleCommand<BuildSystemRequestToProcessCommand>(queueUpSystemRequestCommand, metadata));

            RefreshWorklistCommand refreshWorklistCommand = new RefreshWorklistCommand(instance, contextualData, activityMessage, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshWorklistCommand>(refreshWorklistCommand, metadata));

            RefreshInstanceActivitySecurityCommand refreshInstanceActivitySecurityCommand = new RefreshInstanceActivitySecurityCommand(instance, contextualData, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshInstanceActivitySecurityCommand>(refreshInstanceActivitySecurityCommand, metadata));

            var stageTransitionComment = map.GetStageTransition(instance, contextualData, param, messages);

            // Activity -> get WorkflowId -> from wkflow get processId -> use to get the actual process exposing isLegacy

            RecordStageTransitionCommand recordStageTransitionCommand = new RecordStageTransitionCommand(instance, stageTransitionComment, contextualData, command.UserName, command.Activity, startTime);
            messages.Aggregate(commandHandler.HandleCommand<RecordStageTransitionCommand>(recordStageTransitionCommand, metadata));

            //WorkflowTransitionEvent workflowTransitionEvent = new WorkflowTransitionEvent(
            //    startTime
            //    , 22222
            //    , 22222
            //    ,)
            //RaiseWorkFlowTransitionEventCommand raiseWorkFlowTransitionEventCommand = new RaiseWorkFlowTransitionEventCommand(instance, stageTransitionComment, contextualData, command.UserName, command.Activity, startTime);
            //messages.Aggregate(commandHandler.HandleCommand<SaveStageTransitionCommand>(saveStageTransitionCommand, metadata));

            var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
            messages.Aggregate(commandHandler.HandleCommand(unlockInstanceCommand, metadata));

            return messages;
        }
    }
}