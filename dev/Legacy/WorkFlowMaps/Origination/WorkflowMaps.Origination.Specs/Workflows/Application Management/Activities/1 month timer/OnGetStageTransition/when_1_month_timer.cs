using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities._1_month_timer.OnGetStageTransition
{
    [Subject("Activity => 1_month_timer => OnGetStageTransition")] // AutoGenerated
    internal class when_1_month_timer : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_1_month_timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}