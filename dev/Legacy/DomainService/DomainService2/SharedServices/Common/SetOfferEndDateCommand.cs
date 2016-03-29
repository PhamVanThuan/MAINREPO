namespace DomainService2.SharedServices.Common
{
    public class SetOfferEndDateCommand : StandardDomainServiceCommand
    {
        public SetOfferEndDateCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}