using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States._60_Days_Review.OnEnter
{
    [Subject("State => 60_Days_Review => OnEnter")] // AutoGenerated
    internal class when_60_days_review : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_60_Days_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}