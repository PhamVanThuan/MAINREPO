using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Account;

namespace SAHL.Common.BusinessModel.Specs.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsCheck
{
    [Subject(typeof(ActiveSubsidyAndSalaryStopOrderConditionExistsError))]
    public class when_the_account_has_no_active_subsidy : RulesBaseWithFakes<ActiveSubsidyAndSalaryStopOrderConditionExistsError>
    {
        private static IAccount account;

        private Establish context = () =>
        {
            account = An<IAccount>();
            account.WhenToldTo(x => x.HasActiveSubsidy).Return(false);

            businessRule = new ActiveSubsidyAndSalaryStopOrderConditionExistsError();
            RulesBaseWithFakes<ActiveSubsidyAndSalaryStopOrderConditionExistsError>.startrule.Invoke();
        };

        private Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, account);
        };

        private It should_rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}