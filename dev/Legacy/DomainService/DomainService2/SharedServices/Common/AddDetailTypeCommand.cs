namespace DomainService2.SharedServices.Common
{
    public class AddDetailTypeCommand : StandardDomainServiceCommand
    {
        public AddDetailTypeCommand(int applicationKey, string aDUser, string detailType)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = aDUser;
            this.DetailType = detailType;
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

        public string DetailType
        {
            get;
            protected set;
        }

        public bool Result
        {
            get;
            set;
        }
    }
}