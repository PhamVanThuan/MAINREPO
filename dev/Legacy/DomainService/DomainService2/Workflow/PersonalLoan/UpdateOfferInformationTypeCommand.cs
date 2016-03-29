namespace DomainService2.Workflow.PersonalLoan
{
    public class UpdateOfferInformationTypeCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationTypeKey { get; protected set; }

        public UpdateOfferInformationTypeCommand(int applicationKey, int applicationInformationTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationTypeKey = applicationInformationTypeKey;
        }
    }
}