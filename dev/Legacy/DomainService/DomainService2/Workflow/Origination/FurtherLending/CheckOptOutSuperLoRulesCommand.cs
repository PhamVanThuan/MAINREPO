namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckOptOutSuperLoRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckOptOutSuperLoRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "SuperLoOptOutCheck")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}