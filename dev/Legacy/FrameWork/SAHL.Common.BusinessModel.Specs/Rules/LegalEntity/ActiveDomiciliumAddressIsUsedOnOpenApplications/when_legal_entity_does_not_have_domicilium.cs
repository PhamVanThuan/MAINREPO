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
	public class when_legal_entity_does_not_have_domicilium : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications>
	{
		protected static ILegalEntity legalEntity;
		protected static IApplicationRepository applicationRepository;
		protected static IApplicationRoleDomicilium applicationRoleDomicilium;
		Establish context = () =>
		{
			legalEntity = An<ILegalEntity>();

			applicationRepository = An<IApplicationRepository>();
			applicationRoleDomicilium = An<IApplicationRoleDomicilium>();

			legalEntity.WhenToldTo(x => x.ActiveDomiciliumAddress).Return((ILegalEntityAddress)null);

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
