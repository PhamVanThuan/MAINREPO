using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Common_Callback.OnExit
{
    [Subject("State => Common_Callback => OnExit")] // AutoGenerated
    internal class when_common_callback : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Callback(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}