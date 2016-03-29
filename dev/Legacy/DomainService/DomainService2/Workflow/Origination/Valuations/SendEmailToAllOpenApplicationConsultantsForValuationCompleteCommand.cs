namespace DomainService2.Workflow.Origination.Valuations
{
    public class SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand : StandardDomainServiceCommand
    {
        public SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand(int valuationKey, int applicationKey)
        {
            this.ValuationKey = valuationKey;
            this.ApplicationKey = applicationKey;
        }

        public int ValuationKey
        {
            get;
            set;
        }

        public int ApplicationKey
        {
            get;
            set;
        }
    }
}