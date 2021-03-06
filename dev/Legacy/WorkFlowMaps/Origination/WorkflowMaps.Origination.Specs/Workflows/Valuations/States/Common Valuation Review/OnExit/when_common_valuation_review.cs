using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Common_Valuation_Review.OnExit
{
    [Subject("State => Common_Valuation_Review => OnExit")] // AutoGenerated
    internal class when_common_valuation_review : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Valuation_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}