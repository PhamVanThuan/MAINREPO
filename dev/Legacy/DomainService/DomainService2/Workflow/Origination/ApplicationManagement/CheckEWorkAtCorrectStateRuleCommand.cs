namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckEWorkAtCorrectStateRuleCommand : RuleDomainServiceCommand
    {
        public CheckEWorkAtCorrectStateRuleCommand(int applicationkey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.CheckEWorkInResubmitted)
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; set; }
    }
}