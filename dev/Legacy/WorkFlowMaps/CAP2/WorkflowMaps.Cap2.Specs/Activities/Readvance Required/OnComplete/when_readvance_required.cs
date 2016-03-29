using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Readvance_Required.OnComplete
{
    [Subject("Activity => Readvance_Required => OnComplete")]
    internal class when_readvance_required : WorkflowSpecCap2
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Readvance_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}