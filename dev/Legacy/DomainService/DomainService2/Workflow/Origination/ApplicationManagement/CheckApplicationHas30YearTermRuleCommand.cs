using SAHL.Common;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckApplicationHas30YearTermRuleCommand : RuleDomainServiceCommand
    {
        public CheckApplicationHas30YearTermRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, Rules.ApplicationHas30YearTerm)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}