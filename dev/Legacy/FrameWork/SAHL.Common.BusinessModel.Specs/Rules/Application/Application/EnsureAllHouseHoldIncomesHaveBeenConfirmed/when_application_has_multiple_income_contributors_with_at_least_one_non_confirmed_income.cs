using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.EnsureAllHouseHoldIncomesHaveBeenConfirmed
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome))]
    public class when_application_has_multiple_income_contributors_with_at_least_one_non_confirmed_income : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome>
    {
        static IApplication application;
        static IReadOnlyEventList<IApplicationRole> applicationRoles;
        static IApplicationRole firstIncomeContributorRole;
        static IApplicationRole secondIncomeContributorRole;

        static IApplicationRoleAttribute incomeContributorRoleAttribute;
        static IApplicationRoleAttributeType incomeContributorAttributeType;
        static ILegalEntity legalEntityFirstContributor;
        static ILegalEntity legalEntitySecondContributor;

        static IEmployment firstContributorEmployment;
        static IEmploymentStatus firstContributorEmploymentWithConfirmedIncomeStatus;
        static IEmployment secondContributorEmployment;
        static IEmployment secondContributorAdditionalEmployment;

        static IEmploymentStatus secondContributorEmploymentWithConfirmedIncomeStatus;

        Establish context = () =>
        {
            incomeContributorAttributeType = An<IApplicationRoleAttributeType>();
            incomeContributorAttributeType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);

            incomeContributorRoleAttribute = An<IApplicationRoleAttribute>();
            incomeContributorRoleAttribute.WhenToldTo(x => x.OfferRoleAttributeType).Return(incomeContributorAttributeType);

            firstContributorEmploymentWithConfirmedIncomeStatus = An<IEmploymentStatus>();
            firstContributorEmploymentWithConfirmedIncomeStatus.WhenToldTo(x => x.Key).Return((int)EmploymentStatuses.Current);

            secondContributorEmploymentWithConfirmedIncomeStatus = An<IEmploymentStatus>();
            secondContributorEmploymentWithConfirmedIncomeStatus.WhenToldTo(x => x.Key).Return((int)EmploymentStatuses.Current);

            firstContributorEmployment = An<IEmployment>();
            firstContributorEmployment.WhenToldTo(x => x.EmploymentStatus).Return(firstContributorEmploymentWithConfirmedIncomeStatus);
            firstContributorEmployment.WhenToldTo(x => x.ConfirmedBasicIncome).Return(1.0d);

            secondContributorEmployment = An<IEmployment>();
            secondContributorEmployment.WhenToldTo(x => x.EmploymentStatus).Return(secondContributorEmploymentWithConfirmedIncomeStatus);
            secondContributorEmployment.WhenToldTo(x => x.ConfirmedBasicIncome).Return(1.0d);

            secondContributorAdditionalEmployment = An<IEmployment>();
            secondContributorAdditionalEmployment.WhenToldTo(x => x.EmploymentStatus).Return(secondContributorEmploymentWithConfirmedIncomeStatus);
            secondContributorAdditionalEmployment.WhenToldTo(x => x.ConfirmedBasicIncome).Return(0.0d);

            legalEntityFirstContributor = An<ILegalEntity>();
            legalEntityFirstContributor.WhenToldTo(x => x.Employment).Return(new EventList<IEmployment>(new List<IEmployment>() { firstContributorEmployment }));

            legalEntitySecondContributor = An<ILegalEntity>();
            legalEntitySecondContributor.WhenToldTo(x => x.Employment).Return(new EventList<IEmployment>(new List<IEmployment>() { secondContributorEmployment, secondContributorAdditionalEmployment }));

            firstIncomeContributorRole = An<IApplicationRole>();
            firstIncomeContributorRole.WhenToldTo(x => x.ApplicationRoleAttributes).Return(new EventList<IApplicationRoleAttribute>(new List<IApplicationRoleAttribute>() { incomeContributorRoleAttribute }));
            firstIncomeContributorRole.WhenToldTo(x => x.LegalEntity).Return(legalEntityFirstContributor);

            secondIncomeContributorRole = An<IApplicationRole>();
            secondIncomeContributorRole.WhenToldTo(x => x.ApplicationRoleAttributes).Return(new EventList<IApplicationRoleAttribute>(new List<IApplicationRoleAttribute>() { incomeContributorRoleAttribute }));
            secondIncomeContributorRole.WhenToldTo(x => x.LegalEntity).Return(legalEntitySecondContributor);

            applicationRoles = new ReadOnlyEventList<IApplicationRole>(new List<IApplicationRole>() { firstIncomeContributorRole, secondIncomeContributorRole });

            application = An<IApplication>();
            application.WhenToldTo(app => app.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client)).Return(applicationRoles);

            businessRule = new BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        It should_fail_rule_check = () =>
        {
            messages.Count.ShouldBeGreaterThan(0);
        };
    }
}
