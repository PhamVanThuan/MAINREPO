using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months))]
	public class when_active_domicilium_is_not_used_on_application : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications>
	{
		protected static ILegalEntity legalEntity;
		protected static ILegalEntityAddress legalEntityDomicilium;
		protected static IApplicationRepository applicationRepository;
		protected static IApplicationRoleDomicilium applicationRoleDomicilium;
		protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
        protected static IApplicationRole applicationRole;
        protected static IApplication application;
		Establish context = () =>
		{
			legalEntity = An<ILegalEntity>();
			legalEntityDomicilium = An<ILegalEntityAddress>();
			legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();

			applicationRepository = An<IApplicationRepository>();
			applicationRoleDomicilium = An<IApplicationRoleDomicilium>();
            application = An<IApplication>();
            applicationRole = An<IApplicationRole>();

            application.WhenToldTo(x => x.IsOpen).Return(true);
            applicationRole.WhenToldTo(x => x.Application).Return(application);
            applicationRoleDomicilium.WhenToldTo(x => x.ApplicationRole).Return(applicationRole);

            legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
			legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
			legalEntity.WhenToldTo(x => x.ActiveDomiciliumAddress).Return(legalEntityDomicilium);
            legalEntityDomicilium.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

			applicationRepository.WhenToldTo(x => x.GetApplicationsThatUseLegalEntityDomicilium(legalEntityDomicilium)).Return(new EventList<IApplicationRoleDomicilium>());

			businessRule = new BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications(applicationRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, legalEntity);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
