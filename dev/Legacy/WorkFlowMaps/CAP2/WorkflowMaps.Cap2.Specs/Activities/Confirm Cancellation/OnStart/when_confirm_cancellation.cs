using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Confirm_Cancellation.OnStart
{
    [Subject("Activity => Confirm_Cancellation => OnStart")]
    internal class when_confirm_cancellation : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Confirm_Cancellation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}