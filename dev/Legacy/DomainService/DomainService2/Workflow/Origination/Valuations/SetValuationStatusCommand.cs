namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationStatusCommand : StandardDomainServiceCommand
    {
        public SetValuationStatusCommand(int valuationKey, int valuationStatusKey)
        {
            this.ValuationKey = valuationKey;
            this.ValuationStatusKey = valuationStatusKey;
        }

        public int ValuationKey { get; set; }

        public int ValuationStatusKey { get; set; }
    }
}