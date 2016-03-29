using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Promotion_Client.OnComplete
{
    [Subject("Activity => Promotion_Client => OnComplete")]
    internal class when_promotion_client : WorkflowSpecCap2
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Promotion_Client(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}