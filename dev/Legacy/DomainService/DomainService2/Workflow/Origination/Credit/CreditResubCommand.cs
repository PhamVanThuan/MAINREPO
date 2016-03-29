namespace DomainService2.Workflow.Origination.Credit
{
    public class CreditResubCommand : StandardDomainServiceCommand
    {
        public CreditResubCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}