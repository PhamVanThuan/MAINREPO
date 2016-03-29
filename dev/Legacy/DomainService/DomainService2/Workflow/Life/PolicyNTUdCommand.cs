namespace DomainService2.Workflow.Life
{
    public class PolicyNTUdCommand : StandardDomainServiceCommand
    {
        public PolicyNTUdCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}