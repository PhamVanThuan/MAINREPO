using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Common_Valuation_Review.OnEnter
{
    [Subject("State => Common_Valuation_Review => OnEnter")] // AutoGenerated
    internal class when_common_valuation_review : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Valuation_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}