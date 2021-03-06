using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Reset_30_Day_Timer.OnComplete
{
    [Subject("Activity => Reset_30_Day_Timer => OnComplete")] // AutoGenerated
    internal class when_reset_30_day_timer : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Reset_30_Day_Timer(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}