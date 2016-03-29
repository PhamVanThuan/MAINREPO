namespace DomainService2.Workflow.Life
{
    public class ActivateLifePolicyCommand : StandardDomainServiceCommand
    {
        public ActivateLifePolicyCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}