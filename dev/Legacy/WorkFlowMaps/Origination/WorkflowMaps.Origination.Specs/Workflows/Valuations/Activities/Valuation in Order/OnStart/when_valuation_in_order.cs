using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_in_Order.OnStart
{
    [Subject("Activity => Valuation_in_Order => OnStart")]
    internal class when_valuation_in_order : WorkflowSpecValuations
    {
        private static bool result;
        private static IValuations client;

        private Establish context = () =>
        {
            client = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(client);
            result = false;
            client.WhenToldTo(x => x.CheckValuationInOrderRules((IDomainMessageCollection)messages, workflowData.ValuationKey, true)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Valuation_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_valuation_in_order_rules = () =>
        {
            client.WasToldTo(x => x.CheckValuationInOrderRules((IDomainMessageCollection)messages, workflowData.ValuationKey, true));
        };

        private It should_return_what_check_valuation_in_order_returns = () =>
        {
            //Mocked IValuations CheckValuationInOrderRules to return true (chose to use true,cause default is false),
            //Testing that OnStartActivity_Valuation_in_Order return true in this scenario.
            result.ShouldBeTrue();
        };
    }
}