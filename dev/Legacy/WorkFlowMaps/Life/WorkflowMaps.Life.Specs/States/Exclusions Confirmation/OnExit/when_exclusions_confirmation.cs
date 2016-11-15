using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Exclusions_Confirmation.OnExit
{
    [Subject("State => Exclusions_Confirmation => OnExit")] // AutoGenerated
    internal class when_exclusions_confirmation : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Exclusions_Confirmation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}