namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveDetailFromApplicationAfterNTUFinalisedCommand : StandardDomainServiceCommand
    {
        public RemoveDetailFromApplicationAfterNTUFinalisedCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}