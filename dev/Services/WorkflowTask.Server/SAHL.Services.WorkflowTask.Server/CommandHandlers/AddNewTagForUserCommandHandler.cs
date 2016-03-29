using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers
{
    public class CreateTagForUserCommandHandler : IServiceCommandHandler<CreateTagForUserCommand>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public CreateTagForUserCommandHandler(IWorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(CreateTagForUserCommand command, IServiceRequestMetadata metadata)
        {
            var messagesToReturn = SystemMessageCollection.Empty();
            string username = metadata.UserName;

            dataManager.AddTagForUser(command);
            messagesToReturn.AddMessage(new SystemMessage(string.Format("Tag {0} has Been Added To Username {1}", command.Caption, username), SystemMessageSeverityEnum.Info));

            return messagesToReturn;
        }
    }
}
