namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class PublisherError
    {
        public PublisherError(string processName, string workflowName, string errorMessage, string category)
        {
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
            this.ErrorMessage = errorMessage;
            this.Category = category;
        }

        public string ProcessName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public string Category { get; protected set; }
    }
}