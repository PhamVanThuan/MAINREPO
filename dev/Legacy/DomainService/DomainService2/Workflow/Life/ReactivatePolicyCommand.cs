namespace DomainService2.Workflow.Life
{
    public class ReactivatePolicyCommand : DomainServiceWithExclusionSetCommand
    {
        public ReactivatePolicyCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}