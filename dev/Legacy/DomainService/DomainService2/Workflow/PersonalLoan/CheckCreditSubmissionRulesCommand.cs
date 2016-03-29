using SAHL.Common;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCreditSubmissionRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckCreditSubmissionRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, RuleSets.PersonalLoanApplicationInOrder)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}