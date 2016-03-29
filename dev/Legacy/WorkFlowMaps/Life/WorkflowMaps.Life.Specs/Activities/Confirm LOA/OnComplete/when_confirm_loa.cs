using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_LOA.OnComplete
{
    [Subject("Activity => Confirm_LOA => OnComplete")] // AutoGenerated
    internal class when_confirm_loa : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Confirm_LOA(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}