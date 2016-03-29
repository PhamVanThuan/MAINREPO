using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Return_to_Manage_Application.OnGetStageTransition
{
    [Subject("Activities => Return_to_Manage_Application => OnGetStageTransition")]
    internal class when_return_to_manage_application : WorkflowSpecReadvancePayments
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            result = string.Empty;
            expectedResult = "Return from Readvance Payments";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Return_to_Manage_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_expected_result_string = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}