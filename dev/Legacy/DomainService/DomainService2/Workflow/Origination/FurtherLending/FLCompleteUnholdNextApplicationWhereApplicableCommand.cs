namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class FLCompleteUnholdNextApplicationWhereApplicableCommand : StandardDomainServiceCommand
    {
        public FLCompleteUnholdNextApplicationWhereApplicableCommand(int applicationkey)
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}