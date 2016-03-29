using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Grant_CAP2_Offer.OnComplete
{
    [Subject("Activity => Grant_CAP2_Offer => OnComplete")]
    internal class when_grant_cap2_offer : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            message = string.Empty;
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Grant_CAP2_Offer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}