using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities._45_days.OnStart
{
    [Subject("Activity => _30_day_timer => OnStart")] // AutoGenerated
    internal class when_45_days : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_30_Day_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}