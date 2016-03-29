using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline___Offer.OnStart
{
    [Subject("Activity => Decline_+_Offer => OnStart")]
    internal class when_decline__offer_and_action_source_is_not_decline_with_offer : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = "Approve with Pricing Changes";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline___Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}