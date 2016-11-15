using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Term_Change_Request.OnGetStageTransition
{
    [Subject("Activity => Term_Change_Request => OnGetStageTransition")] // AutoGenerated
    internal class when_term_change_request : WorkflowSpecLoanAdjustments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Term_Change_Request(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}