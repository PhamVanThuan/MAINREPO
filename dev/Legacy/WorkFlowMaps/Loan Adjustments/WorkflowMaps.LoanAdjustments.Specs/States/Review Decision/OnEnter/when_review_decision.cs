using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Review_Decision.OnEnter
{
    [Subject("State => Review_Decision => OnEnter")] // AutoGenerated
    internal class when_review_decision : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Review_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}