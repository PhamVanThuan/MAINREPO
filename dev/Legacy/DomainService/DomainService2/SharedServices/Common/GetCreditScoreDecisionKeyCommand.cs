namespace DomainService2.SharedServices.Common
{
    public class GetCreditScoreDecisionKeyCommand : StandardDomainServiceCommand
    {
        public GetCreditScoreDecisionKeyCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int Result
        {
            get;
            set;
        }
    }
}