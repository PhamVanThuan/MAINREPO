using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.InvalidAppHold.OnEnter
{
    [Subject("State => InvalidAppHold => OnEnter")] // AutoGenerated
    internal class when_invalidapphold : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_InvalidAppHold(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}