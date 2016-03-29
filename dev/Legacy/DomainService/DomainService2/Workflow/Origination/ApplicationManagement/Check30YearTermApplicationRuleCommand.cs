using SAHL.Common;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class Check30YearTermApplicationRuleCommand : RuleDomainServiceCommand
    {
        public Check30YearTermApplicationRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, Rules.ApplicationCannotHave30YearTerm)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}