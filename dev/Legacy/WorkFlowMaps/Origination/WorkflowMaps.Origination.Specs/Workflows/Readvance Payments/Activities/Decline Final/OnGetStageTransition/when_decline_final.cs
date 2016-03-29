using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Decline_Final.OnGetStageTransition
{
    [Subject("Activity => Decline Final => OnGetStageTransition")]
    internal class when_decline_final : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_Final(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_decline_final_string = () =>
        {
            result.ShouldEqual("Decline Final");
        };
    }
}