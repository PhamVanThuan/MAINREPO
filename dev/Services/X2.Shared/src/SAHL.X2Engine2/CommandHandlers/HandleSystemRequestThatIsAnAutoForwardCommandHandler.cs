using System;
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
    public class HandleSystemRequestThatIsAnAutoForwardCommandHandler : IServiceCommandHandler<HandleSystemRequestThatIsAnAutoForwardCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ProcessProvider processProvider;
        IMessageCollectionFactory messageCollectionFactory;

        public HandleSystemRequestThatIsAnAutoForwardCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
            this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(HandleSystemRequestThatIsAnAutoForwardCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = messageCollectionFactory.CreateEmptyCollection();
            InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);
            WorkFlowDataModel workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);
            StateDataModel stateDataModel = workflowDataProvider.GetStateById((int)instance.StateID);

            DeleteAllScheduleActivitiesCommand deleteScheduledActivityCommand = new DeleteAllScheduleActivitiesCommand(command.InstanceId);
            messages.Aggregate(commandHandler.HandleCommand(deleteScheduledActivityCommand, metadata));

            IX2Process process = processProvider.GetProcessForInstance(instance.ID);
            IX2Map map = process.GetWorkflowMap(workflow.Name);

            IX2ContextualDataProvider contextualDataProvider = map.GetContextualData(instance.ID);
            contextualDataProvider.LoadData(instance.ID);

            IX2Params param = new X2Params(string.Empty, stateDataModel.Name, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            string autoForwardStateName = map.GetForwardStateName(instance, contextualDataProvider, param, messages);
            StateDataModel stateToForwardTo = workflowDataProvider.GetStateDataModel(autoForwardStateName, workflow.Name);
            try
            {
                param = new X2Params(string.Empty, stateToForwardTo.Name, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return SystemMessageCollection.Empty();
            }
            var alertMessage = "Forwarded to State " + stateToForwardTo.Name;
            instance.StateID = stateToForwardTo.ID;
            instance.StateChangeDate = DateTime.Now;

            var result = map.EnterState(instance, contextualDataProvider, param, messages);
            HandleMapReturnCommand handleMapReturnCommand = new HandleMapReturnCommand(command.InstanceId, result, messages, param.ActivityName, WorkflowMapCodeSectionEnum.OnEnter);
            commandHandler.HandleCommand<HandleMapReturnCommand>(handleMapReturnCommand, metadata);
            if (!handleMapReturnCommand.Result)
            {
                return messages;
            }

            SaveContextualDataCommand saveContextualDataCommand = new SaveContextualDataCommand(contextualDataProvider, instance.ID);
            messages.Aggregate(commandHandler.HandleCommand<SaveContextualDataCommand>(saveContextualDataCommand, metadata));

            SaveInstanceCommand saveInstanceCommand = new SaveInstanceCommand(instance);
            messages.Aggregate(commandHandler.HandleCommand<SaveInstanceCommand>(saveInstanceCommand, metadata));

            // We dont do this in the current engine. An AutoForward isnt actually an activity so we dont have an activity record. We may want the publisher to insert "dummy"
            // autoforward activities in the activity table in the future.
            //SaveWorkflowHistoryCommand saveWorkflowHistoryCommand = new SaveWorkflowHistoryCommand(instance, state.ID, command.Activity.ActivityID, command.UserName, DateTime.Now);
            //commandHandler.HandleCommand<SaveWorkflowHistoryCommand>(saveWorkflowHistoryCommand);

            BuildSystemRequestToProcessCommand queueUpSystemRequestCommand = new BuildSystemRequestToProcessCommand(instance, contextualDataProvider);
            messages.Aggregate(commandHandler.HandleCommand<BuildSystemRequestToProcessCommand>(queueUpSystemRequestCommand, metadata));

            RefreshWorklistCommand refreshWorklistCommand = new RefreshWorklistCommand(instance, contextualDataProvider, alertMessage, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshWorklistCommand>(refreshWorklistCommand, metadata));

            RefreshInstanceActivitySecurityCommand refreshInstanceActivitySecurityCommand = new RefreshInstanceActivitySecurityCommand(instance, contextualDataProvider, map);
            messages.Aggregate(commandHandler.HandleCommand<RefreshInstanceActivitySecurityCommand>(refreshInstanceActivitySecurityCommand, metadata));

            command.Result = true;
            return messages;
        }
    }
}