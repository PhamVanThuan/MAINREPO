namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveRegistrationProcessDetailTypesCommand : StandardDomainServiceCommand
    {
        public RemoveRegistrationProcessDetailTypesCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}