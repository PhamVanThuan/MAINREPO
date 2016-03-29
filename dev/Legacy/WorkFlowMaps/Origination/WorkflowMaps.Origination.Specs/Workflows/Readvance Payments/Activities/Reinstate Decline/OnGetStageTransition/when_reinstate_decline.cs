using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reinstate_Decline.OnGetStageTransition
{
    [Subject("Activity => Reinstate_Decline => OnGetStageTransition")]
    internal class when_reinstate_decline : WorkflowSpecReadvancePayments
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            result = string.Empty;
            expectedResult = "Reinstate Decline";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reinstate_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_reinstate_decline_string = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}