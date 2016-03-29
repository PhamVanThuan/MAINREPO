using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers;
using System;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers
{
    public class DeleteTagForUserCommandHandler : IServiceCommandHandler<DeleteTagForUserCommand>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public DeleteTagForUserCommandHandler(IWorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(DeleteTagForUserCommand command, IServiceRequestMetadata metadata)
        {
            var returningMessages = SystemMessageCollection.Empty();
            dataManager.DeleteAllInstancesOfTagOnWorkflowItems(command.Id);
            returningMessages.AddMessage(new SystemMessage("Deleted all instaces of Tag from WorkflowItems", SystemMessageSeverityEnum.Info));
            dataManager.DeleteTagForUser(command.Id);
            returningMessages.AddMessage(new SystemMessage("Deleted Tag From User Tag List", SystemMessageSeverityEnum.Info));
            return returningMessages;
        }
    }
}