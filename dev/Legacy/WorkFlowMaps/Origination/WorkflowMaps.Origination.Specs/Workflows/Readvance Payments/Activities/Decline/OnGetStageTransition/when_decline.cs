using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Decline.OnGetStageTransition
{
    [Subject("Activity => Decline => OnGetStageTransition")]
    internal class when_decline : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_Decline = () =>
        {
            result.ShouldMatch("Decline");
        };
    }
}