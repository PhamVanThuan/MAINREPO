using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Approve_with_Pricing_Changes.OnGetStageTransition
{
    [Subject("Activity => Approve_With_Pricing_Changes => OnGetStageTransition")]
    internal class when_approve_with_pricing_changes : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Approve_with_Pricing_Changes(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_approve_with_pricing_changes = () =>
        {
            result.ShouldBeTheSameAs("Approve with Pricing Changes");
        };
    }
}