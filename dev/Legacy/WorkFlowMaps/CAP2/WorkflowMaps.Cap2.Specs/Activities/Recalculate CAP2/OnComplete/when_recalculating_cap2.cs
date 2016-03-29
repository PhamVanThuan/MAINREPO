using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Recalculate_CAP2.OnComplete
{
    [Subject("Activity => Recalculate_CAP2 => OnComplete")]
    internal class when_recalculating_cap2 : WorkflowSpecCap2
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Recalculate_CAP2(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}