using Machine.Specifications;
using SAHL.Common.BusinessModel.Rules.Application.FurtherLending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.FurtherLending.PreventAlphaHousingLinkRateBeingLowerForFurtherLending
{
    [Subject(typeof(AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate))]
    public class when_the_newly_selected_linkrate_is_lower_than_the_existing_linkrate : RulesBaseWithFakes<AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate>
    {
        static double existingRate;
        static double newlyProposedRate;
        Establish context = () =>
        {
            existingRate = 0.028;
            newlyProposedRate = 0.025;

            businessRule = new AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate();
            RulesBaseWithFakes<AlphaHousingLinkRateCanNotBeLowerThanExistingLinkRate>.startrule.Invoke();
        };

        Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, existingRate, newlyProposedRate);
        };

        It should_rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
