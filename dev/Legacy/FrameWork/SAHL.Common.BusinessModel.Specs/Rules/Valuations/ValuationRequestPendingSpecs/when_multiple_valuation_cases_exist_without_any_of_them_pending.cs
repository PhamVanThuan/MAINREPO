using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Valuations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.ValuationRequestPendingSpecs
{
    [Subject(typeof(ValuationRequestPending))]
    public class when_multiple_valuation_cases_exist_without_any_of_them_pending : RulesBaseWithFakes<ValuationRequestPending>
    {
        protected static IProperty property;
        protected static IValuation nonPendingValuation0, nonPendingValuation1;
        protected static IValuationStatus nonPendingValuationStatus0, nonPendingValuationStatus1;
        protected static int anyNonPendingValuationStatusKey0, anyNonPendingValuationStatusKey1;

        Establish context = () =>
        {
            property = An<IProperty>();
            nonPendingValuation0 = An<IValuation>();
            nonPendingValuation1 = An<IValuation>();
            nonPendingValuationStatus0 = An<IValuationStatus>();
            nonPendingValuationStatus1 = An<IValuationStatus>();
            anyNonPendingValuationStatusKey0 = (int)ValuationStatuses.Withdrawn;
            anyNonPendingValuationStatusKey1 = (int)ValuationStatuses.Returned;

            property.WhenToldTo(x => x.Valuations).Return(new EventList<IValuation>(new IValuation[] { nonPendingValuation0, nonPendingValuation1 }));
            nonPendingValuation0.WhenToldTo(x => x.ValuationStatus).Return(nonPendingValuationStatus0);
            nonPendingValuation1.WhenToldTo(x => x.ValuationStatus).Return(nonPendingValuationStatus1);
            nonPendingValuationStatus0.WhenToldTo(x => x.Key).Return(anyNonPendingValuationStatusKey0);
            nonPendingValuationStatus1.WhenToldTo(x => x.Key).Return(anyNonPendingValuationStatusKey1);

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
