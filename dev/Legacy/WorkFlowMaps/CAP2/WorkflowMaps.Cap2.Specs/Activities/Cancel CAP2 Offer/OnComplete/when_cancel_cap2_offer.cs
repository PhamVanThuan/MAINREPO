using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cancel_CAP2_Offer.OnComplete
{
    [Subject("Activity => Cancel_CAP2_Offer => OnComplete")]
    internal class when_cancel_cap2_offer : WorkflowSpecCap2
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cancel_CAP2_Offer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}