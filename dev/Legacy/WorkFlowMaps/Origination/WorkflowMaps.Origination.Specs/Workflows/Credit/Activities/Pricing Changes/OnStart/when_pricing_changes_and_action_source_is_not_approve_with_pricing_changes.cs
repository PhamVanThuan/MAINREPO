using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Pricing_Changes.OnStart
{
    [Subject("Activity => Pricing_Changes => OnStart")]
    internal class when_pricing_changes_and_action_source_is_not_approve_with_pricing_changes : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = "Decline with Offer";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Pricing_Changes(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}