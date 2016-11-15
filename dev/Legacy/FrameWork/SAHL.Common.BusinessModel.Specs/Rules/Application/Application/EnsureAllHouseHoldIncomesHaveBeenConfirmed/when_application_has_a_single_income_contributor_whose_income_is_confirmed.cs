﻿using Machine.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.EnsureAllHouseHoldIncomesHaveBeenConfirmed
{
    public class when_application_has_a_single_income_contributor_whose_income_is_confirmed : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome>
    {
        static IApplication application;
        static IReadOnlyEventList<IApplicationRole> applicationRoles;
        static IApplicationRole incomeContributorRole;

        static IApplicationRoleAttribute incomeContributorRoleAttribute;
        static IApplicationRoleAttributeType incomeContributorAttributeType;
        static ILegalEntity legalEntity;

        static IEmployment employment;
        static IEmploymentStatus employmentStatus;

        Establish context = () =>
        {
            incomeContributorAttributeType = An<IApplicationRoleAttributeType>();
            incomeContributorAttributeType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);

            incomeContributorRoleAttribute = An<IApplicationRoleAttribute>();
            incomeContributorRoleAttribute.WhenToldTo(x => x.OfferRoleAttributeType).Return(incomeContributorAttributeType);

            employmentStatus = An<IEmploymentStatus>();
            employmentStatus.WhenToldTo(x => x.Key).Return((int)EmploymentStatuses.Current);

            employment = An<IEmployment>();
            employment.WhenToldTo(x => x.EmploymentStatus).Return(employmentStatus);
            employment.WhenToldTo(x => x.ConfirmedBasicIncome).Return(1.0d);

            legalEntity = An<ILegalEntity>();
            legalEntity.WhenToldTo(x => x.Employment).Return(new EventList<IEmployment>(new List<IEmployment>() { employment }));

            incomeContributorRole = An<IApplicationRole>();
            incomeContributorRole.WhenToldTo(x => x.ApplicationRoleAttributes).Return(new EventList<IApplicationRoleAttribute>(new List<IApplicationRoleAttribute>() { incomeContributorRoleAttribute }));
            incomeContributorRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

            applicationRoles = new ReadOnlyEventList<IApplicationRole>(new List<IApplicationRole>() { incomeContributorRole });

            application = An<IApplication>();
            application.WhenToldTo(app => app.GetApplicationRolesByGroup(OfferRoleTypeGroups.Client)).Return(applicationRoles);

            businessRule = new BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonHouseholdContributorConfirmedIncome>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        It should_pass_rule_check = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}