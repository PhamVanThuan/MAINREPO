using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers;
using System;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers
{
    public class AddTagToWorkflowInstanceCommandHandler : IServiceCommandHandler<AddTagToWorkflowInstanceCommand>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public AddTagToWorkflowInstanceCommandHandler(IWorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(AddTagToWorkflowInstanceCommand command, IServiceRequestMetadata metadata)
        {
            var messagesToReturn = new SystemMessageCollection();
            dataManager.AddTagToWorkFlowItem(command);
            messagesToReturn.AddMessage(new SystemMessage(string.Format("Tag {0} has Been Added To workflow Item {1} ", command.TagId, command.WorkflowInstanceId), SystemMessageSeverityEnum.Info));
            return messagesToReturn;
        }
    }
}