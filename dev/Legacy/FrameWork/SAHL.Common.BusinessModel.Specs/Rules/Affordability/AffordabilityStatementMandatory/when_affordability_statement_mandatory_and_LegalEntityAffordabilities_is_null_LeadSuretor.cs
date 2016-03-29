using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Affordability.AffordabilityStatementMandatory
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory))]
    public class when_affordability_statement_mandatory_and_LegalEntityAffordabilities_is_null_LeadSuretor : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory>
    {
        static IApplicationMortgageLoan applicationMortgageLoan;

        Establish Context = () =>
        {
            applicationMortgageLoan = An<IApplicationMortgageLoan>();
            IEventList<IApplicationRole> roles = new EventList<IApplicationRole>();
            IApplicationRole applicationRole = An<IApplicationRole>();

            IApplicationRoleType roleType = An<IApplicationRoleType>();
            roleType.WhenToldTo(x => x.Key).Return(10);

            ILegalEntity legalEntity = An<ILegalEntity>();

            applicationRole.WhenToldTo(x => x.ApplicationRoleType).Return(roleType);
            applicationRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

            roles.Add(messages, applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(roles.AsEnumerable());

            applicationMortgageLoan.WhenToldTo(x => x.ApplicationRoles).Return(applicationRoles);

            businessRule = new BusinessModel.Rules.Affordability.AffordabilityStatementMandatory();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Affordability.AffordabilityStatementMandatory>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, applicationMortgageLoan);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}