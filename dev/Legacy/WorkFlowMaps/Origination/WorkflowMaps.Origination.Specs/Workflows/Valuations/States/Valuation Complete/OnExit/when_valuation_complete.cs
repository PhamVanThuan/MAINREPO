using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Valuation_Complete.OnExit
{
    [Subject("State => Valuation_Complete => OnExit")] // AutoGenerated
    internal class when_valuation_complete : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Valuation_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}