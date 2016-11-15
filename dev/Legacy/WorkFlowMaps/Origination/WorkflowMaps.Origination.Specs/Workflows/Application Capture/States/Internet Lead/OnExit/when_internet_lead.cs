using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Internet_Lead.OnExit
{
    [Subject("State => Internet_Lead => OnExit")] // AutoGenerated
    internal class when_internet_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Internet_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}