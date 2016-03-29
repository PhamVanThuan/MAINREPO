namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckApplicationDebitOrderCollectionRuleCommand : RuleDomainServiceCommand
    {
        public CheckApplicationDebitOrderCollectionRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.ApplicationDebitOrderCollection)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}