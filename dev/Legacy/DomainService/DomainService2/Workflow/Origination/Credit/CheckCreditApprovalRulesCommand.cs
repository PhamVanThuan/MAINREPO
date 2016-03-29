namespace DomainService2.Workflow.Origination.Credit
{
    public class CheckCreditApprovalRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckCreditApprovalRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.CreditApproval)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}