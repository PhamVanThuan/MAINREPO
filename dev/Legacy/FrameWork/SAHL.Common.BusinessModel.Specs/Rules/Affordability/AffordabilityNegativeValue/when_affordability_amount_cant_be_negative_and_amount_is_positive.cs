using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityNegativeValue
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue))]
    public class when_affordability_amount_cant_be_negative_and_amount_is_positive : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue>
    {
        private static ILegalEntityAffordability affordability;
        private static double amount;

        Establish Context = () =>
        {
            amount = 500;

            affordability = An<ILegalEntityAffordability>();
            affordability.WhenToldTo(x => x.Amount).Return(amount);

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityNegativeValue();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, affordability);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
