using System;
using System.Linq;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory))]
    public class when_affordability_description_Mandatory_and_legalentity_affordability_is_wrong_type : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>
    {
        private static ILegalEntity affordability;

        Establish Context = () =>
        {
            affordability = An<ILegalEntity>();

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>.startrule.Invoke();
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