using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Account;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsCheck
{
    [Subject(typeof(ActiveSubsidyAndSalaryStopOrderConditionExistsError))]
    public class when_the_account_has_an_active_subsidy_and_no_conditions : RulesBaseWithFakes<ActiveSubsidyAndSalaryStopOrderConditionExistsError>
    {
        private static IAccount account;
        private static IEventList<IApplication> acceptedApplications;

        private Establish context = () =>
        {
            account = An<IAccount>();
            account.WhenToldTo(x => x.HasActiveSubsidy).Return(true);

            acceptedApplications = new EventList<IApplication>();
            account.WhenToldTo(x => x.Applications).Return(acceptedApplications);

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