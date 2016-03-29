using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Debt_Review_Approved.OnExit
{
    [Subject("State => Debt_Review_Approved => OnExit")] // AutoGenerated
    internal class when_debt_review_approved : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Debt_Review_Approved(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}