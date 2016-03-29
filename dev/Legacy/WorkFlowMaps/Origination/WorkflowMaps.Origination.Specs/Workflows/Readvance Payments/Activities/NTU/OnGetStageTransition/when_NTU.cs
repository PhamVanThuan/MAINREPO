using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.NTU.OnGetStageTransition
{
    [Subject("Activity => NTU => OnGetStageTransition")]
    internal class when_NTU : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_NTU = () =>
        {
            result.ShouldMatch("NTU");
        };
    }
}