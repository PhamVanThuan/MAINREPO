namespace SAHL.Core.X2.Messages
{
    public class QueueNameConstructor
    {
        public string GenerateQueueName(string processName, string workflowName)
        {
            return string.Format("{0}.{1}", processName, workflowName);
        }
    }
}