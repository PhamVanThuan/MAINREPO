namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckSuretyForReAdvanceRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckSuretyForReAdvanceRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "Further Lending Sureties")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}