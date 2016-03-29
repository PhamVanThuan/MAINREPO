namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class RemoveDetailTypesCommand : StandardDomainServiceCommand
    {
        public RemoveDetailTypesCommand(int applicationKey)
            : base()
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}