namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckCreditOverrideRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckCreditOverrideRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationManagementCreditOverrideCheck)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}