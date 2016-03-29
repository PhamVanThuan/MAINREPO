using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Valuations;
using Machine.Fakes;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Valuations.ValuationRequestPendingSpecs
{
    [Subject(typeof(ValuationRequestPending))]
    public class when_no_pending_valuation_case_exists : RulesBaseWithFakes<ValuationRequestPending>
    {
        protected static IValuation valuation;
        protected static IValuationStatus valuationStatus;
        protected static IProperty property;
        protected static int valuationStatusKey;

        Establish context = () =>
        {
            property = An<IProperty>();
            valuation = An<IValuation>();
            valuationStatus = An<IValuationStatus>();
            // Could equally have been any other status with the exception of Pending
            valuationStatusKey = (int)ValuationStatuses.Complete;

            property.WhenToldTo(x => x.Valuations).Return(new EventList<IValuation>(new IValuation[] {valuation}));
            valuation.WhenToldTo(x => x.ValuationStatus).Return(valuationStatus);
            valuationStatus.WhenToldTo(x => x.Key).Return(valuationStatusKey);

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
