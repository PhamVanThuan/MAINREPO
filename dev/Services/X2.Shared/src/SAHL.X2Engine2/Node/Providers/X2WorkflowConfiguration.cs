namespace SAHL.X2Engine2.Node.Providers
{
    public class X2WorkflowConfiguration
    {
        public string WorkflowName { get; protected set; }

        public string ProcessName { get; protected set; }

        public int UserConsumers { get; protected set; }

        public int SystemConsumers { get; protected set; }

        public X2WorkflowConfiguration(string processName, string workflowName, int userConsumers, int systemConsumers)
        {
            this.WorkflowName = workflowName;
            this.ProcessName = processName;
            this.UserConsumers = userConsumers;
            this.SystemConsumers = systemConsumers;
        }
    }
}