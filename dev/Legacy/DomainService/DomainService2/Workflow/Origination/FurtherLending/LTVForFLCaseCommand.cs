namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class LTVForFLCaseCommand : StandardDomainServiceCommand
    {
        public LTVForFLCaseCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public double LTV
        {
            get;
            set;
        }
    }
}