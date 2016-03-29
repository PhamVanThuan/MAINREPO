namespace DomainService2.Workflow.Origination.Credit
{
    public class SendResubMailCommand : StandardDomainServiceCommand
    {
        public SendResubMailCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}