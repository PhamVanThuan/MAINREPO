using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Exceptions_Decline_with_Offer.OnGetStageTransition
{
    [Subject("Activity => Exceptions_Decline_With_Offer => OnGetStageTransition")]
    internal class when_exceptions_decline_with_offer : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Exceptions_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_exceptions_decline_with_offer = () =>
        {
            result.ShouldBeTheSameAs("Exceptions Decline with Offer");
        };
    }
}