namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckRapidShouldGotoCreditRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckRapidShouldGotoCreditRulesCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, "Readvance – Application In Order")
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; protected set; }
    }
}