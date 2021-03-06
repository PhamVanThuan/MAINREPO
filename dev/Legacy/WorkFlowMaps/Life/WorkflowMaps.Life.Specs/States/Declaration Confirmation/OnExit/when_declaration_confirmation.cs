using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Declaration_Confirmation.OnExit
{
    [Subject("State => Declaration_Confirmation => OnExit")] // AutoGenerated
    internal class when_declaration_confirmation : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Declaration_Confirmation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}