using System;
using System.Linq;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityStatementMandatory
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory))]
    public class when_affordability_statement_mandatory_and_applicationMortgageLoan_is_null : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory>
    {
        static ILegalEntityAffordability applicationMortgageLoan;

        Establish Context = () =>
        {
            applicationMortgageLoan = An<ILegalEntityAffordability>();

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityStatementMandatory();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory>.startrule.Invoke();
        };

        static Exception result;

        Because of = () =>
        {
            result = Catch.Exception(() => businessRule.ExecuteRule(messages, applicationMortgageLoan));
        };

        It rule_should_fail = () =>
        {
            result.ShouldBeOfType<ArgumentException>();
        };
    }
}