namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckNTUDeclineFinalRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckNTUDeclineFinalRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "FL - NTU Final/Decline Final")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}