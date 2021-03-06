using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Disagree_With_Decision.OnGetStageTransition
{
    [Subject("Activity => Disagree_With_Decision => OnGetStageTransition")] // AutoGenerated
    internal class when_disagree_with_decision : WorkflowSpecLoanAdjustments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Disagree_With_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}