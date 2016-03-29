using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Rollback_Disbursement.OnGetStageTransition
{
    [Subject("State => Rollback_Disbursement => OnGetStageTransition")]
    internal class when_rollback_disbursement : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Rollback_Disbursement(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_rollback_disbursement = () =>
        {
            result.ShouldMatch("Rollback Disbursement");
        };
    }
}