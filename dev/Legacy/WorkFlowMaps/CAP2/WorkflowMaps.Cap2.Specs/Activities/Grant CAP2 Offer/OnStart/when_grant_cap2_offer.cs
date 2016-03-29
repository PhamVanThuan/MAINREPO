using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Grant_CAP2_Offer.OnStart
{
    [Subject("Activity => Grant_CAP2_Offer => OnStart")]
    internal class when_grant_cap2_offer : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Grant_CAP2_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}