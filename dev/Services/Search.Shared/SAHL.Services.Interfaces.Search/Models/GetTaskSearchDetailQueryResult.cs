using System;
namespace SAHL.Services.Interfaces.Search.Models
{
    public class GetTaskSearchDetailQueryResult
    {
        public GetTaskSearchDetailQueryResult(DateTime workflowTaskAge, DateTime stateAge, long? ParentTaskId, string parentTaskWorkflowName, string sourceWorkflowName)
        {
            this.WorkflowTaskAge        = workflowTaskAge;
            this.StateAge               = stateAge;
            this.ParentTaskId           = ParentTaskId;
            this.ParentTaskWorkflowName = parentTaskWorkflowName;
            this.SourceWorkflowName     = sourceWorkflowName;
        }

        public DateTime WorkflowTaskAge { get; protected set; }

        public DateTime StateAge { get; protected set; }

        public long? ParentTaskId { get; protected set; }

        public string ParentTaskWorkflowName { get; protected set; }

        public string SourceWorkflowName { get; protected set; }
    }
}
