namespace DomainService2.SharedServices.Common
{
    public class DoCreditScoreCommand : StandardDomainServiceCommand
    {
        public DoCreditScoreCommand(int applicationKey, int callingContextKey, string aDUserName, System.Boolean ignoreWarnings)
            : base(ignoreWarnings)
        {
            this.ApplicationKey = applicationKey;
            this.CallingContextKey = callingContextKey;
            this.ADUserName = aDUserName;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int CallingContextKey
        {
            get;
            protected set;
        }

        public string ADUserName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}