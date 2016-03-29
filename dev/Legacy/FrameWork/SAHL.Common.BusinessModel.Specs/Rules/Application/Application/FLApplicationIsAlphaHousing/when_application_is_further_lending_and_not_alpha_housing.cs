using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using Machine.Fakes;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.FurtherLendingAccountForApplicationIsAlphaHousing
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing))]
	public class when_application_is_further_lending_and_not_alpha_housing : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing>
    {
		protected static IApplication application;
		protected static IAccount account;

		Establish Context = () =>
		{
			application = An<IApplicationFurtherLending>();
			account = An<IAccount>();

			IEventList<IDetail> details = new EventList<IDetail>(new IDetail[] {  });

			account.WhenToldTo(x => x.Details).Return(details);
			application.WhenToldTo(x => x.Account).Return(account);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing>.startrule.Invoke();

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
