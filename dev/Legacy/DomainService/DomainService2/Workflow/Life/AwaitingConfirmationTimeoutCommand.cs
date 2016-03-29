namespace DomainService2.Workflow.Life
{
    public class AwaitingConfirmationTimeoutCommand : StandardDomainServiceCommand
    {
        public AwaitingConfirmationTimeoutCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}