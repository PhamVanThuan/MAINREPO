namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class AddDetailTypesCommand : StandardDomainServiceCommand
    {
        public AddDetailTypesCommand(int applicationKey, string adUser)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = adUser;
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
    }
}