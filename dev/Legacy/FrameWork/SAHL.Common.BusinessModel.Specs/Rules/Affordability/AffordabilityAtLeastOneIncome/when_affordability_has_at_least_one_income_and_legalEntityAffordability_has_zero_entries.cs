using System;
using System.Linq;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityAtLeastOneIncome
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome))]
    public class when_affordability_has_at_least_one_income_and_legalEntityAffordability_has_zero_entries : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>
    {
        static IEventList<ILegalEntityAffordability> affordability;
        static IApplication appilcation;

        Establish Context = () =>
        {
            affordability = An<IEventList<ILegalEntityAffordability>>();
            appilcation = An<IApplication>();

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>.startrule.Invoke();
        };

        static int result;

        Because of = () =>
        {
            result = businessRule.ExecuteRule(messages, affordability, appilcation);
        };

        It rule_should_fail = () =>
        {
            result.ShouldEqual(1);
            messages.Count.ShouldEqual(0);
        };
    }
}