namespace DomainService2.SharedServices.Common
{
    public class CreateAccountForApplicationCommand : StandardDomainServiceCommand
    {
        public CreateAccountForApplicationCommand(int applicationKey, string adUserName)
        {
            this.ApplicationKey = applicationKey;
            this.ADUserName = adUserName;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string ADUserName
        {
            get;
            protected set;
        }
    }
}