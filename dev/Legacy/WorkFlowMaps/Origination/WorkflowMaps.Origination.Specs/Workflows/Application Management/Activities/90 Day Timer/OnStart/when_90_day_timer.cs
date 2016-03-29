using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities._90_Day_Timer.OnStart
{
    [Subject("Activity => 90_Day_Timer => OnStart")] // AutoGenerated
    internal class when_90_day_timer : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_90_Day_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}