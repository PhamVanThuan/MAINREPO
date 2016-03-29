using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers
{
    public class UpdateUserTagCommandHandler : IServiceCommandHandler<UpdateUserTagCommand>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public UpdateUserTagCommandHandler(IWorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateUserTagCommand command, IServiceRequestMetadata metadata)
        {
            var returningMessages = SystemMessageCollection.Empty();

            dataManager.UpdateUserTag(command);
            returningMessages.AddMessage(new SystemMessage("Updated Tag", SystemMessageSeverityEnum.Info));
            return returningMessages;
        }
    }
}