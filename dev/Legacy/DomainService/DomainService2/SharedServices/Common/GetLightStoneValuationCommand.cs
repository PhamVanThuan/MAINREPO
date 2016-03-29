namespace DomainService2.SharedServices.Common
{
    public class GetLightStoneValuationCommand : StandardDomainServiceCommand
    {
        public GetLightStoneValuationCommand(int applicationKey, string aDUser, System.Boolean ignoreWarnings)
            : base(ignoreWarnings)
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