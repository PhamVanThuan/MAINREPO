namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckValuationRequiredRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckValuationRequiredRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationManagementValuationRequired)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}