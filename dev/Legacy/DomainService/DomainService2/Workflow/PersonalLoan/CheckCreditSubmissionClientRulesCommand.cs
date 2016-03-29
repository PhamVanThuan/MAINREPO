using SAHL.Common;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCreditSubmissionClientRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckCreditSubmissionClientRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, RuleSets.PersonalLoanApplicationInOrderClient)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}