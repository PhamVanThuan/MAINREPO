using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Continue_with_Sale.OnComplete
{
    [Subject("Activity => Continue_with_Sale => OnComplete")] // AutoGenerated
    internal class when_continue_with_sale : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Continue_with_Sale(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}