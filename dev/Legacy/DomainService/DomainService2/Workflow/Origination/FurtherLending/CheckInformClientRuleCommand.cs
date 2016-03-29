namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckInformClientRuleCommand : RuleDomainServiceCommand
    {
        public CheckInformClientRuleCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "ProductSuperLoFLSPVChange")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}