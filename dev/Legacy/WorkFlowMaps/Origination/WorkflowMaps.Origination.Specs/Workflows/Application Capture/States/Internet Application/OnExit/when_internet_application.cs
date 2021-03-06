using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Internet_Application.OnExit
{
    [Subject("State => Internet_Application => OnExit")] // AutoGenerated
    internal class when_internet_application : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Internet_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}