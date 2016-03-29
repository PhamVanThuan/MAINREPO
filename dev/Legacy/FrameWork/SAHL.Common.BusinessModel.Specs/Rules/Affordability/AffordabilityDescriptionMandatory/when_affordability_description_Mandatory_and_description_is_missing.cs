using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory))]
    public class when_affordability_description_Mandatory_and_description_is_missing : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>
    {
        private static ILegalEntityAffordability affordability;
        private static IAffordabilityType affordabilityType;

        Establish Context = () =>
        {
            affordabilityType = An<IAffordabilityType>();
            affordabilityType.WhenToldTo(x => x.DescriptionRequired).Return(true);

            affordability = An<ILegalEntityAffordability>();
            affordability.WhenToldTo(x => x.AffordabilityType).Return(affordabilityType);

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityDescriptionMandatory>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, affordability);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}