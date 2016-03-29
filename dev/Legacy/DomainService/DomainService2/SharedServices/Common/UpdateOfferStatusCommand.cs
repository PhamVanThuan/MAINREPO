namespace DomainService2.SharedServices.Common
{
    public class UpdateOfferStatusCommand : StandardDomainServiceCommand
    {
        public UpdateOfferStatusCommand(int applicationKey, int offerStatusKey, int offerInformationTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.OfferStatusKey = offerStatusKey;
            this.OfferInformationTypeKey = offerInformationTypeKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int OfferStatusKey
        {
            get;
            protected set;
        }

        public int OfferInformationTypeKey
        {
            get;
            protected set;
        }
    }
}