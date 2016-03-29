namespace SAHL.X2Engine2.ViewModels
{
    public class ProcessWorkflowViewModel
    {
        public string ProcessName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public ProcessWorkflowViewModel()
        {
        }

        public ProcessWorkflowViewModel(string processName, string workflowName)
        {
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
        }
    }
}