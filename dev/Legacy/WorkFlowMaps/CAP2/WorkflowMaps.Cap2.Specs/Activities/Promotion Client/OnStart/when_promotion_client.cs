using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Promotion_Client.OnStart
{
    [Subject("Activity => Promotion_Client => OnStart")] // AutoGenerated
    internal class when_promotion_client : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Promotion_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}