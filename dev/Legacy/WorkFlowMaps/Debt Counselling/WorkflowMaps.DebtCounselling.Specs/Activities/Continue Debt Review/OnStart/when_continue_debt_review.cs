using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Continue_Debt_Review.OnStart
{
    [Subject("Activity => Continue_Debt_Review => OnStart")] // AutoGenerated
    internal class when_continue_debt_review : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Continue_Debt_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}