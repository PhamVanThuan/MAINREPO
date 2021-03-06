using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Term_Change_Request.OnEnter
{
    [Subject("State => Term_Change_Request => OnEnter")] // AutoGenerated
    internal class when_term_change_request : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Term_Change_Request(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}