using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Awaiting_Timeout.OnExit
{
    [Subject("State => Awaiting_Timeout => OnExit")] // AutoGenerated
    internal class when_awaiting_timeout : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Awaiting_Timeout(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}