namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckQACompleteRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckQACompleteRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationManagementQAComplete)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}