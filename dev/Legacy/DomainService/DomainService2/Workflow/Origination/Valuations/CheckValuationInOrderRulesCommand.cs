namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckValuationInOrderRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckValuationInOrderRulesCommand(int valuationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ValuationsValuationInOrder)
        {
            this.ValuationKey = valuationKey;
        }

        public int ValuationKey { get; set; }
    }
}