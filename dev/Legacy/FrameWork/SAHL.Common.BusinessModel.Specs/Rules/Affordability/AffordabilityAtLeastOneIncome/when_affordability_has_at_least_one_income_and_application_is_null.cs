using System;
using System.Linq;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityAtLeastOneIncome
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome))]
    public class when_affordability_has_at_least_one_income_and_application_is_null : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>
    {
        static IEventList<ILegalEntityAffordability> affordability;
        static IEventList<ILegalEntityAffordability> appilcation;

        Establish Context = () =>
        {
            affordability = An<IEventList<ILegalEntityAffordability>>();
            appilcation = An<IEventList<ILegalEntityAffordability>>();

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>.startrule.Invoke();
        };

        static Exception result;

        Because of = () =>
        {
            result = Catch.Exception(() => businessRule.ExecuteRule(messages, affordability, appilcation));
        };

        It rule_should_fail = () =>
        {
            result.ShouldBeOfType<ArgumentException>();
        };
    }
}