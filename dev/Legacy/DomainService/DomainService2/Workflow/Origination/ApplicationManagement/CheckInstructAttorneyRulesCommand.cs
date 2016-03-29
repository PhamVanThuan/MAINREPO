using SAHL.Common;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckInstructAttorneyRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckInstructAttorneyRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, RuleSets.ApplicationManagementInstructAttorney)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}