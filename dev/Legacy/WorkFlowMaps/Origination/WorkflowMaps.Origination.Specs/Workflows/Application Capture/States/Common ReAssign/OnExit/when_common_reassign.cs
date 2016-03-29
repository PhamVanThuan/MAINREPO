using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_ReAssign.OnExit
{
    [Subject("State => Common_ReAssign => OnExit")] // AutoGenerated
    internal class when_common_reassign : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_ReAssign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}