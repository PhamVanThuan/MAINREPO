using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Rework_Offer.OnComplete
{
    [Subject("Activity => Rework_Offer => OnComplete")]
    internal class when_reworking_offer : WorkflowSpecCap2
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
            result = workflow.OnCompleteActivity_Rework_Offer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}