using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Dispute_Indicated.OnGetStageTransition
{
    [Subject("Activity => Dispute_Indicated => OnGetStageTransition")]
    internal class when_dispute_indicated : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Dispute_Indicated(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_review_required_due_to_dispute_indicated = () =>
        {
            result.ShouldBeTheSameAs("Review required due to dispute indicated.");
        };
    }
}