using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities._45_day_timer.OnStart
{
    [Subject("Activity => 45_day_timer => OnStart")] // AutoGenerated
    internal class when_45_day_timer : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_45_day_timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}