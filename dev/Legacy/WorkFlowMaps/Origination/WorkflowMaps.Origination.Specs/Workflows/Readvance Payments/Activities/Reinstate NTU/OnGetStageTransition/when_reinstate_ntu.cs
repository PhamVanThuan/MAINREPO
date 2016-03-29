using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reinstate_NTU.OnGetStageTransition
{
    [Subject("Activity => Reinstate_NTU => OnGetStageTransition")]
    internal class when_reinstate_ntu : WorkflowSpecReadvancePayments
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            result = string.Empty;
            expectedResult = "Reinstate NTU";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_reinstate_ntu_string = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}