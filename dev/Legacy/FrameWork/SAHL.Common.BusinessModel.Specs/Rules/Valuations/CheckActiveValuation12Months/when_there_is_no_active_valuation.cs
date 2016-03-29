using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.CheckActiveValuation12Months
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12Months))]
	public class when_there_is_no_active_valuation : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12Months>
    {
        private static IApplicationMortgageLoan applicationMortgageLoan;

        Establish context = () =>
        {
            IValuation valuation1 = An<IValuation>();
            valuation1.WhenToldTo(x => x.IsActive).Return(false);

            IValuation valuation2 = An<IValuation>();
            valuation2.WhenToldTo(x => x.IsActive).Return(false);

            IProperty property = An<IProperty>();
            var valuationsEventList = new EventList<IValuation>(new[] { valuation1, valuation2 });
            property.WhenToldTo(x => x.Valuations).Return(valuationsEventList);

			applicationMortgageLoan = An<IApplicationMortgageLoan>();
			applicationMortgageLoan.WhenToldTo(x => x.Property).Return(property);

            businessRule = new BusinessModel.Rules.Valuations.CheckActiveValuation12Months();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12Months>.startrule.Invoke();
        };

        Because of = () =>
        {
			businessRule.ExecuteRule(messages, applicationMortgageLoan);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}