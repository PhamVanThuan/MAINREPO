using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Valuations_Review.OnStart
{
    [Subject("Activity => Valuations_Review => OnStart")] // AutoGenerated
    internal class when_valuations_review : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Valuations_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}