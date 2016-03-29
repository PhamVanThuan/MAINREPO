using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Confirm_Cancellation.OnComplete
{
    [Subject("Activity => Confirm_Cancellation => OnComplete")]
    internal class when_confirm_cancellation : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            message = string.Empty;
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Confirm_Cancellation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}