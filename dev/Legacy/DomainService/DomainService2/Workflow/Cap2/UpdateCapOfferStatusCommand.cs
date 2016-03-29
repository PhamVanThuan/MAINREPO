namespace DomainService2.Workflow.Cap2
{
    public class UpdateCapOfferStatusCommand : StandardDomainServiceCommand
    {
        public UpdateCapOfferStatusCommand(int applicationKey, int statusKey)
        {
            this.ApplicationKey = applicationKey;
            this.StatusKey = statusKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int StatusKey
        {
            get;
            protected set;
        }
    }
}