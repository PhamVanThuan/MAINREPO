using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityAtLeastOneIncome
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome))]
    public class when_affordability_has_at_least_one_income_and_application_keys_dont_match : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>
    {
        static IEventList<ILegalEntityAffordability> affordability;
        static IApplication appilcation;
        static IApplication application2;

        Establish Context = () =>
        {
            IAffordabilityType affordabilityType = An<IAffordabilityType>();
            affordabilityType.WhenToldTo(x => x.IsExpense).Return(false);

            appilcation = An<IApplication>();
            appilcation.WhenToldTo(x => x.Key).Return(1);

            application2 = An<IApplication>();
            application2.WhenToldTo(x => x.Key).Return(2);

            ILegalEntityAffordability legalEntityAffordability = An<ILegalEntityAffordability>();
            legalEntityAffordability.WhenToldTo(x => x.Application).Return(application2);
            legalEntityAffordability.WhenToldTo(x => x.AffordabilityType).Return(affordabilityType);

            affordability = new EventList<ILegalEntityAffordability>();
            affordability.Add(messages, legalEntityAffordability);

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
            messages.Count.ShouldEqual(1);
        };
    }
}