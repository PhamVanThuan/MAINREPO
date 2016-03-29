using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Approve_Rapid.OnGetStageTransition
{
    [Subject("Activity => Approve_Rapid => OnGetStageTransition")]
    internal class when_approve_rapid : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Approve_Rapid(instanceData, workflowData, paramsData, messages);
        };

        private It should_approve_rapid_string = () =>
        {
            result.ShouldBeTheSameAs("Approve Rapid");
        };
    }
}