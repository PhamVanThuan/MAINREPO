namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckBranchSubmitApplicationRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckBranchSubmitApplicationRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationCaptureSubmitApplication)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}