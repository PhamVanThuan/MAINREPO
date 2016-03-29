namespace SAHL.Core.X2.Messages
{
    public class X2Workflow
    {
        private string queueName;

        public string WorkflowName { get; protected set; }

        public string ProcessName { get; protected set; }

        public X2Workflow(string processName, string workflowName)
        {
            this.WorkflowName = workflowName;
            this.ProcessName = processName;
            var queueNameConstrutor = new QueueNameConstructor();
            this.queueName = queueNameConstrutor.GenerateQueueName(ProcessName, WorkflowName);
        }

        public override string ToString()
        {
            return this.queueName;
        }
    }
}