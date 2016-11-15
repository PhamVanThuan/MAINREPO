using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._60_days.OnComplete
{
    [Subject("Activity => 60_days => OnComplete")] // AutoGenerated
    internal class when_60_days : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_60_days(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}