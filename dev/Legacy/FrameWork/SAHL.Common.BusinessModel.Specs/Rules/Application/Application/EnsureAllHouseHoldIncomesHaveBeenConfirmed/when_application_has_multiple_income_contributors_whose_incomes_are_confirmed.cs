using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.EnsureAllHouseHoldIncomesHaveBeenConfirmed
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome))]
    public class when_application_has_multiple_income_contributors_whose_incomes_are_confirmed : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome>
    {
        private static IApplication application;
        private static IReadOnlyEventList<IApplicationRole> applicationRoles;
        private static IApplicationRole firstIncomeContributorRole;
        private static IApplicationRole secondIncomeContributorRole;

        private static IApplicationRoleAttribute incomeContributorRoleAttribute;
        private static IApplicationRoleAttributeType incomeContributorAttributeType;
        private static ILegalEntity legalEntityFirstContributor;
        private static ILegalEntity legalEntitySecondContributor;

        private static IEmployment firstContributorEmployment;
        private static IEmploymentStatus firstContributorEmploymentWithConfirmedIncomeStatus;
        private static IEmployment secondContributorEmployment;
        private static IEmploymentStatus secondContributorEmploymentWithConfirmedIncomeStatus;

        private Establish context = () =>
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

            legalEntityFirstContributor = An<ILegalEntity>();
            legalEntityFirstContributor.WhenToldTo(x => x.Employment).Return(new EventList<IEmployment>(new List<IEmployment>() { firstContributorEmployment }));

            legalEntitySecondContributor = An<ILegalEntity>();
            legalEntitySecondContributor.WhenToldTo(x => x.Employment).Return(new EventList<IEmployment>(new List<IEmployment>() { secondContributorEmployment }));

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

        private Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        private It should_pass_rule_check = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}