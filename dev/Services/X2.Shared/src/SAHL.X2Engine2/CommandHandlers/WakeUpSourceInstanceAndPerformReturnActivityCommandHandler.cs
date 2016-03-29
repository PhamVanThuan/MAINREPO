using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class WakeUpSourceInstanceAndPerformReturnActivityCommandHandler : IServiceCommandHandler<WakeUpSourceInstanceAndPerformReturnActivityCommand>
    {
        public IX2ServiceCommandRouter CommandHandler { get; protected set; }

        public IWorkflowDataProvider WorkflowDataProvider { get; protected set; }

        public WakeUpSourceInstanceAndPerformReturnActivityCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider)
        {
            this.CommandHandler = commandHandler;
            this.WorkflowDataProvider = workflowDataProvider;
        }

        public ISystemMessageCollection HandleCommand(WakeUpSourceInstanceAndPerformReturnActivityCommand command, IServiceRequestMetadata metadata)
        {
            SystemMessageCollection messages = new SystemMessageCollection();
            var workflow = WorkflowDataProvider.GetWorkflowById(command.Instance.WorkFlowID);
            IX2Params param = new X2Params(command.Activity.ActivityName, command.Activity.ToStateName, workflow.Name, command.IgnoreWarnings, command.UserName, metadata, command.Data);
            bool result = command.Map.OnReturnState(command.Instance, command.ContextualDataProvider, param, messages);
            if (result)
            {
                if (command.Instance.SourceInstanceID != null)
                {
                    var sourceInstance = WorkflowDataProvider.GetInstanceDataModel((long)command.Instance.SourceInstanceID);
                    var state = WorkflowDataProvider.GetStateById((int)command.Instance.StateID);
                    if (null != state.ReturnWorkflowID && (int)state.ReturnWorkflowID == sourceInstance.WorkFlowID)
                    {
                        ActivityDataModel returnActivity = WorkflowDataProvider.GetActivity((int)state.ReturnActivityID);
                        List<string> activitiesToProcess = new List<string> { returnActivity.Name };
                        NotificationOfNewSystemRequestGroupCommand notifyEngineOfNewSystemRequestGroupCommand = new NotificationOfNewSystemRequestGroupCommand(activitiesToProcess, sourceInstance.ID);
                        PublishBundledRequestCommand bundledRequestCommand = new PublishBundledRequestCommand(new List<IX2BundledNotificationCommand>() { notifyEngineOfNewSystemRequestGroupCommand });
                        CommandHandler.QueueUpCommandToBeProcessed(bundledRequestCommand);
                    }
                }
            }
            return messages;
            /*
			 * It should_call_onreturnstate_on_the_map = () =>
		   It should_get_the_source_instance=()=>
		   It should_get_the_state_the_remote_instance_is_transitioning_to = () =>
		   It should_queue_up_a_system_request_group_with_one_activity_for_the_source_instance=()=>
		   It should_send_the_system_request_group_tp_the_engine = () =>
			 */
        }
    }
}