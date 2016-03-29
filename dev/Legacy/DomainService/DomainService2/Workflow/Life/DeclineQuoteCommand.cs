namespace DomainService2.Workflow.Life
{
    public class DeclineQuoteCommand : DomainServiceWithExclusionSetCommand
    {
        public DeclineQuoteCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}