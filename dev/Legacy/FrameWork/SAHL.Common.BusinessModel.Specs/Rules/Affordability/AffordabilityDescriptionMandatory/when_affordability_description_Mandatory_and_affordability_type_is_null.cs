using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory))]
    public class when_affordability_description_Mandatory_and_affordability_type_is_null : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>
    {
        private static ILegalEntityAffordability affordability;
        private static IAffordabilityType affordabilityType;

        Establish Context = () =>
        {
            affordabilityType = null;

            affordability = An<ILegalEntityAffordability>();
            affordability.WhenToldTo(x => x.AffordabilityType).Return(affordabilityType);

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>.startrule.Invoke();
        };

        private static int result;

        Because of = () =>
        {
            result = businessRule.ExecuteRule(messages, affordability);
        };

        It rule_should_fail = () =>
        {
            result.ShouldEqual(1);
            messages.Count.ShouldEqual(0);
        };
    }
}