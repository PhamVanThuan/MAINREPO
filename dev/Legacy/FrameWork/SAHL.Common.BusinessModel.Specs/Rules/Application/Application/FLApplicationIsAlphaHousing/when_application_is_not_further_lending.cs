using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.FurtherLendingAccountForApplicationIsAlphaHousing
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing))]
	public class when_application_is_not_further_lending : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing>
    {
        protected static IApplication application;

        Establish Context = () =>
        {
            application = An<IApplication>();

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
