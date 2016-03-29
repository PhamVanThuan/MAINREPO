using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cancel_CAP2_Offer.OnStart
{
    [Subject("Activity => Cancel_CAP2_Offer => OnStart")]
    internal class when_cancel_cap2_offer : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Cancel_CAP2_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}