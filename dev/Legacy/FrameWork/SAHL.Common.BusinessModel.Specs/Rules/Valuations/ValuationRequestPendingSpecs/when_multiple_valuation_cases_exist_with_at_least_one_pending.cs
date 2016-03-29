using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Rules.Valuations;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.ValuationRequestPendingSpecs
{
    [Subject(typeof(ValuationRequestPending))]
    public class when_multiple_valuation_cases_exist_with_at_least_one_pending : RulesBaseWithFakes<ValuationRequestPending>
    {
        protected static IValuation pendingValuation, nonPendingValuation;
        protected static IValuationStatus pendingValuationStatus, nonPendingValuationStatus;
        protected static IProperty property;
        protected static int pendingValuationStatusKey, anyNonPendingValuationStatusKey;

        Establish context = () =>
        {
            property = An<IProperty>();
            pendingValuation = An<IValuation>();
            nonPendingValuation = An<IValuation>();
            pendingValuationStatus = An<IValuationStatus>();
            nonPendingValuationStatus = An<IValuationStatus>();
            pendingValuationStatusKey = (int)ValuationStatuses.Pending;
            anyNonPendingValuationStatusKey = (int)ValuationStatuses.Returned;

            property.WhenToldTo(x => x.Valuations).Return(new EventList<IValuation>(new IValuation[] { pendingValuation, nonPendingValuation }));
            pendingValuation.WhenToldTo(x => x.ValuationStatus).Return(pendingValuationStatus);
            nonPendingValuation.WhenToldTo(x => x.ValuationStatus).Return(nonPendingValuationStatus);
            pendingValuationStatus.WhenToldTo(x => x.Key).Return(pendingValuationStatusKey);
            nonPendingValuationStatus.WhenToldTo(x => x.Key).Return(anyNonPendingValuationStatusKey);

            businessRule = new ValuationRequestPending();
            RulesBaseWithFakes<ValuationRequestPending>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, property);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}

