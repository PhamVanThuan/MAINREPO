namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckCreditSubmissionPrimaryITCRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckCreditSubmissionPrimaryITCRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.CreditScoring)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}