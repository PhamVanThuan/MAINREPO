using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Refer_to_Credit.OnGetStageTransition
{
    [Subject("Activity => Refer_to_Credit => OnGetStageTransition")]
    internal class when_refer_to_credit : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Refer_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_Refer_to_Credit = () =>
        {
            result.ShouldMatch("Refer to Credit");
        };
    }
}