namespace DomainService2.Workflow.Origination.Valuations
{
    public class RecalcHOCCommand : StandardDomainServiceCommand
    {
        public RecalcHOCCommand(int valuationKey, int applicationKey, bool ignoreWarnings)
        {
            this.ValuationKey = valuationKey;
            this.ApplicationKey = applicationKey;
            this.IgnoreWarnings = ignoreWarnings;
        }

        public int ValuationKey { get; set; }

        public int ApplicationKey { get; set; }
    }
}