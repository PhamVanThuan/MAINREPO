using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Quote.OnExit
{
    [Subject("State => Quote => OnExit")] // AutoGenerated
    internal class when_quote : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Quote(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}