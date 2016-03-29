using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.WorkflowTask.Commands
{
    public class DeleteTagFromWorkflowInstanceCommand : ServiceCommand, IWorkflowServiceCommand
    {
        public DeleteTagFromWorkflowInstanceCommand(Guid tagId, long workflowInstanceId)
        {
            TagId = tagId;
            WorkflowInstanceId = workflowInstanceId;
        }

        public Guid TagId { get; protected set; }

        public Int64 WorkflowInstanceId { get; protected set; }
    }
}