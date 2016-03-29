using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers;
using System.Xml.Schema;

namespace SAHL.Services.WorkflowTask.Server.CommandHandlers
{
    public class DeleteTagFromWorkflowInstanceCommandHandler : IServiceCommandHandler<DeleteTagFromWorkflowInstanceCommand>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public DeleteTagFromWorkflowInstanceCommandHandler(IWorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleCommand(DeleteTagFromWorkflowInstanceCommand command, IServiceRequestMetadata metadata)
        {
            var returningMessages = SystemMessageCollection.Empty();
            int keyId = 0;
            keyId = dataManager.GetWorkflowUserTagKey(command.TagId, command.WorkflowInstanceId);
            dataManager.DeleteWorkflowUserTagByKey(keyId);
            returningMessages.AddMessage(new SystemMessage(string.Format("Deleted the Tag {0} attached to {1}", command.TagId, command.WorkflowInstanceId), SystemMessageSeverityEnum.Info));
            return returningMessages;
        }
    }
}