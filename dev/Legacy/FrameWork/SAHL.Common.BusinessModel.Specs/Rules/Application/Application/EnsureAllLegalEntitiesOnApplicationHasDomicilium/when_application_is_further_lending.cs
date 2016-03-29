using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium))]
	public class when_application_is_further_lending : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium>
	{
		protected static IApplicationFurtherLending application;
		protected static IApplicationType applicationType;
		Establish Context = () =>
		{
			application = An<IApplicationFurtherLending>();
			businessRule = new BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium>.startrule.Invoke();
		};
		Because of = () =>
		{
			businessRule.ExecuteRule(messages, application);
		};
		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
