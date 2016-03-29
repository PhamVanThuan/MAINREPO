namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class InitialFLNTUCommand : StandardDomainServiceCommand
    {
        public InitialFLNTUCommand(int applicationKey, string adUser, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = adUser;
            this.InstanceID = instanceID;
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

        public long InstanceID
        {
            get;
            protected set;
        }
    }
}