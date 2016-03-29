namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckValuationExistsRecentRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckValuationExistsRecentRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ValuationsAutoValuation)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}