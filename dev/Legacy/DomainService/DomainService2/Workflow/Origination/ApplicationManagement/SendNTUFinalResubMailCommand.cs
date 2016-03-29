namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendNTUFinalResubMailCommand : StandardDomainServiceCommand
    {
        public SendNTUFinalResubMailCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}