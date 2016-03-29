namespace DomainService2.SharedServices.Common
{
    public class CheckApplicationMinimumIncomeRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckApplicationMinimumIncomeRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.RuleSets.ApplicationMinimumIncome)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}