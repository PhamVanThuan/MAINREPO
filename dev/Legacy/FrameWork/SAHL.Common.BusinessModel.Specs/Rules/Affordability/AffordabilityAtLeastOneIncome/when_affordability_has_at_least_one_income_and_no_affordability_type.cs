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
    public class when_affordability_has_at_least_one_income_and_no_affordability_type : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityAtLeastOneIncome>
    {
        static IEventList<ILegalEntityAffordability> affordability;
        static IApplication appilcation;

        Establish Context = () =>
        {
            appilcation = An<IApplication>();
            appilcation.WhenToldTo(x => x.Key).Return(Param.IsAny<int>);

            ILegalEntityAffordability legalEntityAffordability = An<ILegalEntityAffordability>();
            legalEntityAffordability.WhenToldTo(x => x.Application).Return(appilcation);
            legalEntityAffordability.WhenToldTo(x => x.AffordabilityType).Return((IAffordabilityType)null);

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