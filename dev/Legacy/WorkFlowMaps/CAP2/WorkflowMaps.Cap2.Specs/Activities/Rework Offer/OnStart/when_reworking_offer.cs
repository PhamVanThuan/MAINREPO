using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Rework_Offer.OnStart
{
    [Subject("Activity => Rework_Offer => OnStart")]
    internal class when_reworking_offer : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rework_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}