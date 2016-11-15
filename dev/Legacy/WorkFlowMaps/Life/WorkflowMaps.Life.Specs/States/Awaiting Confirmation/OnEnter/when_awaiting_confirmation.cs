using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Awaiting_Confirmation.OnEnter
{
    [Subject("State => Awaiting_Confirmation => OnEnter")] // AutoGenerated
    internal class when_awaiting_confirmation : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Awaiting_Confirmation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}