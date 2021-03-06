using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._60_days.OnGetStageTransition
{
    [Subject("Activity => 60_days => OnGetStageTransition")] // AutoGenerated
    internal class when_60_days : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_60_days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}