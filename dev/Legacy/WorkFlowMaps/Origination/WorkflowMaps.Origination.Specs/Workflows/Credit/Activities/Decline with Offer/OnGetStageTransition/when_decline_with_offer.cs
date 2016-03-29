using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline_with_Offer.OnGetStageTransition
{
    [Subject("Activity => Decline_With_Offer => OnGetStagetransition")]
    internal class when_decline_with_offer : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline_with_offer = () =>
        {
            result.ShouldBeTheSameAs("Decline with Offer");
        };
    }
}