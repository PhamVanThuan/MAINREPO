using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Surety_check_for_Rapid.OnGetStageTransition
{
    [Subject("State => Surety_Check_For_Rapid => OnGetStageTransition")]
    internal class when_surety_check_for_rapid : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Surety_check_for_Rapid(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_Surety_check_for_Rapid = () =>
        {
            result.ShouldMatch("Surety check for Rapid");
        };
    }
}