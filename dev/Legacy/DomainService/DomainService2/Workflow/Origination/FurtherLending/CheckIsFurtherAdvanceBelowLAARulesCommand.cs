namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckIsFurtherAdvanceBelowLAARulesCommand : RuleSetDomainServiceCommand
    {
        public CheckIsFurtherAdvanceBelowLAARulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "RP_FurtherAdvanceBelowLAA")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}