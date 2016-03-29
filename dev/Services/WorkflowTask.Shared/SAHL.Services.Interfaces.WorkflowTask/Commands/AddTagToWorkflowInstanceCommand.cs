using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.WorkflowTask.Commands
{
    public class AddTagToWorkflowInstanceCommand : ServiceCommand, IWorkflowServiceCommand
    {
        public AddTagToWorkflowInstanceCommand(Guid tagId, long workflowInstanceId, string username)
        {
            this.TagId = tagId;
            this.WorkflowInstanceId = workflowInstanceId;
            Username = username;
        }

        public Int64 WorkflowInstanceId { get; protected set; }

        public Guid TagId { get; protected set; }

        public string Username { get; protected set; }
    }
}