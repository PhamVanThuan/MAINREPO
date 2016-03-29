using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Surety_Check_for_FA.OnGetStageTransition
{
    [Subject("Activity => Surety_Check_for_FA => GetStageTransition")]
    internal class when_surety_check_for_fa : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Surety_Check_for_FA(instanceData, workflowData, paramsData, messages);
        };

        private It should_Surety_Check_for_FA = () =>
        {
            result.ShouldBeTheSameAs("Surety Check for FA");
        };
    }
}