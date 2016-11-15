using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Wait_for_Callback.OnStart
{
    [Subject("Activity => Wait_for_Callback => OnStart")] // AutoGenerated
    internal class when_wait_for_callback : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Wait_for_Callback(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}