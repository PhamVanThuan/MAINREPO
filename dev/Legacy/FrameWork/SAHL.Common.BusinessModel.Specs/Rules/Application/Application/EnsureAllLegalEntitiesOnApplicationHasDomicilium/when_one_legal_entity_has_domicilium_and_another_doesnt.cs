using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium))]
	public class when_one_legal_entity_has_domicilium_and_another_doesnt : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium>
	{
		protected static IApplication application;
		protected static IApplicationRole applicationRole;
		protected static IApplicationRole anotherApplicationRole;
		protected static ILegalEntity legalEntity;
		protected static IApplicationRoleDomicilium applicationRoleDomicilium;

        protected static IApplicationRoleType applicationRoleType;
        protected static IApplicationRoleTypeGroup applicationRoleTypeGroup;

		Establish Context = () =>
		{
			application = An<IApplication>();
			applicationRole = An<IApplicationRole>();
			anotherApplicationRole = An<IApplicationRole>();
			applicationRoleDomicilium = An<IApplicationRoleDomicilium>();

            applicationRoleTypeGroup = An<IApplicationRoleTypeGroup>();
            applicationRoleTypeGroup.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);
            applicationRoleType = An<IApplicationRoleType>();
            applicationRoleType.WhenToldTo(x => x.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);
            applicationRoleType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant);
            applicationRole.WhenToldTo(x => x.ApplicationRoleType).Return(applicationRoleType);
            anotherApplicationRole.WhenToldTo(x => x.ApplicationRoleType).Return(applicationRoleType);

			applicationRole.WhenToldTo(x => x.ApplicationRoleDomicilium).Return(applicationRoleDomicilium);
			anotherApplicationRole.WhenToldTo(x => x.ApplicationRoleDomicilium).Return((IApplicationRoleDomicilium)null);

			ReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(new IApplicationRole[] { applicationRole, anotherApplicationRole });

			application.WhenToldTo(x => x.ApplicationRoles).Return(applicationRoles);

			businessRule = new BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium>.startrule.Invoke();
		};
		Because of = () =>
		{
			businessRule.ExecuteRule(messages, application);
		};
		It rule_should_fail = () =>
		{
			messages.Count.ShouldEqual(1);
		};
	}
}
