using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.isStandardCover.OnComplete
{
    [Subject("Activity => isStandardCover => OnComplete")] // AutoGenerated
    internal class when_isstandardcover : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_isStandardCover(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}