using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Rollback_Disbursement.OnGetStageTransition
{
    [Subject("Activity => Rollback_Disbursement => OnGetStageTransition")]
    internal class when_rollback_disbursement : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Rollback_Disbursement(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_rollback_disbursement_stagetransition = () =>
        {
            result.ShouldBeTheSameAs("Rollback Disbursement");
        };
    }
}