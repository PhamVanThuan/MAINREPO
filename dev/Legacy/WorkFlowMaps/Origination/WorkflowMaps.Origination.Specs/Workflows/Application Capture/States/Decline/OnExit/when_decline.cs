using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Decline.OnExit
{
    [Subject("State => Decline => OnExit")]
    internal class when_decline : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}