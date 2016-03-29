namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationActiveAndSaveCommand : StandardDomainServiceCommand
    {
        public SetValuationActiveAndSaveCommand(int valuationKey)
        {
            this.ValuationKey = valuationKey;
        }

        public int ValuationKey { get; set; }
    }
}