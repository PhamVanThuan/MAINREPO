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
	public class when_active_domicilium_is_used_on_application_that_is_closed : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications>
	{
		protected static ILegalEntity legalEntity;
		protected static ILegalEntityAddress legalEntityDomicilium;
		protected static IApplicationRepository applicationRepository;
		protected static IApplicationRoleDomicilium applicationRoleDomicilium;
		protected static IEventList<IApplicationRoleDomicilium> applicationRoleDomiciliums;
		protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
		protected static IApplicationRole applicationRole;
		protected static IApplication application;
		protected static IApplicationStatus applicationStatus;
		Establish context = () =>
		{
			legalEntity = An<ILegalEntity>();
			legalEntityDomicilium = An<ILegalEntityAddress>();
			legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();

			application = An<IApplication>();
			applicationRole = An<IApplicationRole>();
			applicationStatus = An<IApplicationStatus>();

			applicationRepository = An<IApplicationRepository>();
			applicationRoleDomicilium = An<IApplicationRoleDomicilium>();

			applicationStatus.WhenToldTo(x => x.Key).Return((int)OfferStatuses.Closed);
			application.WhenToldTo(x => x.ApplicationStatus).Return(applicationStatus);
			application.WhenToldTo(x => x.Key).Return(1);

			applicationRole.WhenToldTo(x => x.Application).Return(application);
			applicationRoleDomicilium.WhenToldTo(x => x.ApplicationRole).Return(applicationRole);

			legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
			legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
			legalEntity.WhenToldTo(x => x.ActiveDomiciliumAddress).Return(legalEntityDomicilium);

			applicationRoleDomiciliums = new EventList<IApplicationRoleDomicilium>(new[] { applicationRoleDomicilium });
			applicationRepository.WhenToldTo(x => x.GetApplicationsThatUseLegalEntityDomicilium(legalEntityDomicilium)).Return(applicationRoleDomiciliums);

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