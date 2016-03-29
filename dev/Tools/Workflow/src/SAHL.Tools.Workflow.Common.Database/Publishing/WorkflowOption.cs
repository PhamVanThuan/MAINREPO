namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class WorkflowOption
    {
        public WorkflowOption(string workflowName)
        {
            this.WorkflowName = workflowName;
        }

        public string WorkflowName { get; protected set; }
    }
}