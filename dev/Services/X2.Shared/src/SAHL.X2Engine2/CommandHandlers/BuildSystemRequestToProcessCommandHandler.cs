using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.CommandHandlers
{
	public class BuildSystemRequestToProcessCommandHandler : IServiceCommandHandler<BuildSystemRequestToProcessCommand>
	{
		private IWorkflowDataProvider workflowDataProvider;
		private IX2ProcessProvider processProvider;
        private IX2ServiceCommandRouter commandHandler;
        IMessageCollectionFactory messageCollectionFactory;

        public BuildSystemRequestToProcessCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider, IX2ProcessProvider processProvider, IMessageCollectionFactory messageCollectionFactory)
		{
			this.commandHandler = commandHandler;
			this.workflowDataProvider = workflowDataProvider;
			this.processProvider = processProvider;
            this.messageCollectionFactory = messageCollectionFactory;
		}

        public ISystemMessageCollection HandleCommand(BuildSystemRequestToProcessCommand command, IServiceRequestMetadata metadata)
		{
            var messages = messageCollectionFactory.CreateEmptyCollection();
            PublishBundledRequestCommand bundledRequestCommand = new PublishBundledRequestCommand();
			StateDataModel stateDataModel = workflowDataProvider.GetStateById((int)command.Instance.StateID);
			if (stateDataModel.ForwardState)
			{
                NotificationOfNewAutoForwardCommand notifyEngineOfNewAutoForwardCommand = new NotificationOfNewAutoForwardCommand(command.Instance.ID);
                bundledRequestCommand.Commands.Add(notifyEngineOfNewAutoForwardCommand);
			}
			else
			{
				IEnumerable<ActivityDataModel> systemActivities = workflowDataProvider.GetSystemActivitiesForState((int)command.Instance.StateID);
				DeleteAllScheduleActivitiesCommand deleteScheduledActivityCommand = new DeleteAllScheduleActivitiesCommand(command.Instance.ID);
                messages.Aggregate(commandHandler.HandleCommand<DeleteAllScheduleActivitiesCommand>(deleteScheduledActivityCommand, metadata));
                var process = processProvider.GetProcessForInstance(command.Instance.ID);
                var map = process.GetWorkflowMap(workflowDataProvider.GetWorkflowName(command.Instance));
                command.ContextualData.LoadData(command.Instance.ID);
				foreach (ActivityDataModel activityDataModel in systemActivities.OrderBy(x => x.Priority))
				{
					if (activityDataModel.Type == (int)Enumerations.ActivityTypes.Decision)
					{
						InsertScheduledActivityCommand insertScheduledActivityCommand = new InsertScheduledActivityCommand(command.Instance.ID, DateTime.Now, activityDataModel, string.Empty);
                        commandHandler.HandleCommand<InsertScheduledActivityCommand>(insertScheduledActivityCommand, metadata);
						command.DecisionsToProcess.Add(activityDataModel.Name);
					}
					else
					{
                        if (activityDataModel.Type == (int)Enumerations.ActivityTypes.Timed)
						{
							var workflow = workflowDataProvider.GetWorkflowById(command.Instance.WorkFlowID);
                            IX2Params param = new X2Params(activityDataModel.Name, stateDataModel.Name, workflow.Name, true, "X2", null);

							DateTime TimerExecutionTime = map.GetActivityTime(command.Instance, command.ContextualData, param, messages);

							InsertScheduledActivityCommand insertScheduledActivityCommand = new InsertScheduledActivityCommand(command.Instance.ID, TimerExecutionTime, activityDataModel, string.Empty);
                            commandHandler.HandleCommand<InsertScheduledActivityCommand>(insertScheduledActivityCommand, metadata);

							if (TimerExecutionTime > DateTime.Now)
							{
                                NotificationOfNewFutureScheduledActivityCommand notifyEngineOfNewFutureTimedActivityCommand = new NotificationOfNewFutureScheduledActivityCommand(command.Instance.ID, activityDataModel.ID);
                                commandHandler.QueueUpCommandToBeProcessed(notifyEngineOfNewFutureTimedActivityCommand);
							}
							else
							{
                                NotificationOfNewScheduledActivityCommand notifyEngineOfNewTimedActivityCommand = new NotificationOfNewScheduledActivityCommand(command.Instance.ID, activityDataModel.ID);
                                bundledRequestCommand.Commands.Add(notifyEngineOfNewTimedActivityCommand);
							}
						}
					}
				}
			}
			IEnumerable<WorkFlowActivityDataModel> workflowActivities = workflowDataProvider.GetWorkflowActivitiesForState((int)command.Instance.StateID);
			foreach (var workflowActivity in workflowActivities)
			{
                NotificationOfNewWorkflowActivityCommand notifyEngineOfNewWorkflowActivityCommand = new NotificationOfNewWorkflowActivityCommand(command.Instance.ID, workflowActivity.ID);
                bundledRequestCommand.Commands.Add(notifyEngineOfNewWorkflowActivityCommand);
			}
            if (command.DecisionsToProcess.Count > 0)
            {
                NotificationOfNewSystemRequestGroupCommand notifyEngineOfNewSystemRequestGroupCommand = new NotificationOfNewSystemRequestGroupCommand(command.DecisionsToProcess, command.Instance.ID);
                bundledRequestCommand.Commands.Add(notifyEngineOfNewSystemRequestGroupCommand);
            }
            commandHandler.QueueUpCommandToBeProcessed(bundledRequestCommand);
            return messages;
		}

    }
}