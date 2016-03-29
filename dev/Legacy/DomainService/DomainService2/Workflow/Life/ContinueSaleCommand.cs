namespace DomainService2.Workflow.Life
{
    public class ContinueSaleCommand : StandardDomainServiceCommand
    {
        public ContinueSaleCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public bool Result { get; set; }
    }
}