using System;
using System.Linq;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityNegativeValue
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue))]
    public class when_affordability_amount_cant_be_negative_and_legalentity_affordability_is_wrong_type : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue>
    {
        private static ILegalEntity affordability;

        Establish Context = () =>
        {
            affordability = An<ILegalEntity>();

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityNegativeValue();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityNegativeValue>.startrule.Invoke();
        };

        private static Exception result;

        Because of = () =>
        {
            result = Catch.Exception(() => businessRule.ExecuteRule(messages, affordability));
        };

        It rule_should_fail = () =>
        {
            result.ShouldBeOfType<ArgumentException>();
        };
    }
}