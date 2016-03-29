namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveAccountFromRegMailCommand : StandardDomainServiceCommand
    {
        public RemoveAccountFromRegMailCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}