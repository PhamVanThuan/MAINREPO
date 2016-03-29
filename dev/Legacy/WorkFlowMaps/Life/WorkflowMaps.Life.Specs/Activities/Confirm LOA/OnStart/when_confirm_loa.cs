using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_LOA.OnStart
{
    [Subject("Activity => Confirm_LOA => OnStart")] // AutoGenerated
    internal class when_confirm_loa : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Confirm_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}