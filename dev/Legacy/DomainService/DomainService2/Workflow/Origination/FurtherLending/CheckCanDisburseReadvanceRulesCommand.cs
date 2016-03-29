namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckCanDisburseReadvanceRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckCanDisburseReadvanceRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "FL - Disbursement Complete")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}