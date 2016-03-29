namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class IsFurtherAdvanceApplicationCommand : StandardDomainServiceCommand
    {
        public IsFurtherAdvanceApplicationCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public bool Result { get; set; }
    }
}