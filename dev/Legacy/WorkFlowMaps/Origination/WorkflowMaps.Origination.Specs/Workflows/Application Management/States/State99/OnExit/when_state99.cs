using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.State99.OnExit
{
    [Subject("State => State99 => OnExit")] // AutoGenerated
    internal class when_state99 : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_State99(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}