namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckSuperLoOptOutRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckSuperLoOptOutRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "SuperLoOptOutCheck")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}