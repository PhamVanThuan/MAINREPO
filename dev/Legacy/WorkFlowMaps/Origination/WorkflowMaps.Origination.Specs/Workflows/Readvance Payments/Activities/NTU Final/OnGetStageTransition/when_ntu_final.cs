using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.NTU_Final.OnGetStageTransition
{
    [Subject("Activity => NTU_Final => OnGetStageTransition")]
    internal class when_ntu_final : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU_Final(instanceData, workflowData, paramsData, messages);
        };

        private It should_ntu_final_string = () =>
        {
            result.ShouldEqual("NTU Final");
        };
    }
}