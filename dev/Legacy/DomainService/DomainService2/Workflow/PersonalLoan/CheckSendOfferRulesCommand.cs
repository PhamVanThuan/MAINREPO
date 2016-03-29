using SAHL.Common;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckSendOfferRulesCommand : RuleSetDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public CheckSendOfferRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, RuleSets.PersonalLoanSendOffer)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}