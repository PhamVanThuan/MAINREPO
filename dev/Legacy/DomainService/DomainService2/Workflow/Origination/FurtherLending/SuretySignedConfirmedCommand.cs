namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class SuretySignedConfirmedCommand : StandardDomainServiceCommand
    {
        public SuretySignedConfirmedCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}