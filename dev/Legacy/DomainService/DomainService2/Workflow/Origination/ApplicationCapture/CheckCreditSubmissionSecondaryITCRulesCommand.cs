namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckCreditSubmissionSecondaryITCRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckCreditSubmissionSecondaryITCRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.CreditScoring)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}