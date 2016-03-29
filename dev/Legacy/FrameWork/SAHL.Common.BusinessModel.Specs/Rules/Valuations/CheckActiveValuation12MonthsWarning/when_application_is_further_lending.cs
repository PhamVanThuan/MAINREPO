using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.CheckActiveValuation12MonthsWarning
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12MonthsWarning))]
    public class when_application_is_further_lending : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12MonthsWarning>
    {
        private static IApplicationFurtherLending furtherLendingApplication;

        Establish context = () =>
        {
            IValuation valuation = An<IValuation>();
            valuation.WhenToldTo(x => x.IsActive).Return(false);

            IProperty property = An<IProperty>();
            var valuationsEventList = new EventList<IValuation>(new[] { valuation });
            property.WhenToldTo(x => x.Valuations).Return(valuationsEventList);

			furtherLendingApplication = An<IApplicationFurtherLending>();

            businessRule = new BusinessModel.Rules.Valuations.CheckActiveValuation12MonthsWarning();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12MonthsWarning>.startrule.Invoke();
        };

        Because of = () =>
        {
			businessRule.ExecuteRule(messages, furtherLendingApplication);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}