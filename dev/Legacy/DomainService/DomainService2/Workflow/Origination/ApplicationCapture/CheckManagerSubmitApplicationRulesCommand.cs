namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckManagerSubmitApplicationRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckManagerSubmitApplicationRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationCaptureManagerSubmitApplication)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}