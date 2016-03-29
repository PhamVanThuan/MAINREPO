namespace DomainService2.Workflow.Life
{
    public class NTUPolicyCommand : DomainServiceWithExclusionSetCommand
    {
        public NTUPolicyCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}