namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckResubOverRideRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckResubOverRideRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationManagementResubOverRideCheck)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}