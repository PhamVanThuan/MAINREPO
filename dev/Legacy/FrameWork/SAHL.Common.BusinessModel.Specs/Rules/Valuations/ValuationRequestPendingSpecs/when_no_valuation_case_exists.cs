using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Valuations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.ValuationRequestPendingSpecs
{
    [Subject(typeof(ValuationRequestPending))]
    public class when_no_valuation_case_exists : RulesBaseWithFakes<ValuationRequestPending>
    {
        protected static IProperty property;

        Establish context = () =>
        {
            property = An<IProperty>();
            property.WhenToldTo(x => x.Valuations).Return(new EventList<IValuation>(new IValuation[] {}));

            businessRule = new ValuationRequestPending();
            RulesBaseWithFakes<ValuationRequestPending>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, property);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
