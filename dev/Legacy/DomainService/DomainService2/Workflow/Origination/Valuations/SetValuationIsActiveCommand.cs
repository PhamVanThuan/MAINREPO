namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationIsActiveCommand : StandardDomainServiceCommand
    {
        public int ValuationKey { get; set; }

        public SetValuationIsActiveCommand(int valuationKey)
        {
            this.ValuationKey = valuationKey;
        }
    }
}