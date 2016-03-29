using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_in_Order.OnComplete
{
    [Subject("Activity => Valuation_in_Order => OnComplete")]
    internal class when_valuation_in_order : WorkflowSpecValuations
    {
        private static string message;
        private static bool result;
        private static IValuations client;

        private Establish context = () =>
        {
            client = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Valuation_in_Order(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_recalculate_HOC = () =>
        {
            client.WasToldTo(x => x.RecalcHOC((IDomainMessageCollection)messages, workflowData.ValuationKey, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}