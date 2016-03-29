namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckSuperLoOptOutRequiredRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckSuperLoOptOutRequiredRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "RP_RequireOptOutOfSuperLo")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}