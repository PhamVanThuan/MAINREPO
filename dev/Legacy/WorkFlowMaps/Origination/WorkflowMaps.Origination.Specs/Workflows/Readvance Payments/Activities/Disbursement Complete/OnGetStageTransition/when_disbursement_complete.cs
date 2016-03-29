using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Disbursement_Complete.OnGetStageTransition
{
    [Subject("Activity => Disbursement_Complete => OnGetStageTransition")]
    internal class when_disbursement_complete : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Disbursement_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_Disbursement_Complete = () =>
        {
            result.ShouldMatch("Disbursement Complete");
        };
    }
}