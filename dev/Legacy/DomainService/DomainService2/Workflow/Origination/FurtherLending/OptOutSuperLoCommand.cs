namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class OptOutSuperLoCommand : StandardDomainServiceCommand
    {
        public OptOutSuperLoCommand(int applicationKey, string aDUser)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = aDUser;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string ADUser
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}