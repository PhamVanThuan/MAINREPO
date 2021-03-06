using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Completed_Expired.OnStart
{
    [Subject("Activity => Completed_Expired => OnStart")] // AutoGenerated
    internal class when_completed_expired : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Completed_Expired(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}