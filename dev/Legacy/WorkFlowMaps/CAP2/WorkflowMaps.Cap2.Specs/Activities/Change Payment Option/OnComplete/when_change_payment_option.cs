using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Change_Payment_Option.OnComplete
{
    [Subject("Activity => Change_Payment_Option => OnComplete")]
    internal class when_change_payment_option : WorkflowSpecCap2
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Change_Payment_Option(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}