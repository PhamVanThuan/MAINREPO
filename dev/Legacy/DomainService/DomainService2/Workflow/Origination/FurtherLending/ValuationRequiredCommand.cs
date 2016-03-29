namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class ValuationRequiredCommand : StandardDomainServiceCommand
    {
        public ValuationRequiredCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}