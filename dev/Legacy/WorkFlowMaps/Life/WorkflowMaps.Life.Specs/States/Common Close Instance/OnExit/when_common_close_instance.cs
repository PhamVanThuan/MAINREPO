using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Common_Close_Instance.OnExit
{
    [Subject("State => Common_Close_Instance => OnExit")] // AutoGenerated
    internal class when_common_close_instance : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Close_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}