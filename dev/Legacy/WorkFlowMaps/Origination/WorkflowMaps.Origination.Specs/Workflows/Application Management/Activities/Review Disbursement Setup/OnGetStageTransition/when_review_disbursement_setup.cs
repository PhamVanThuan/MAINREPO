using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Disbursement_Setup.OnGetStageTransition
{
    [Subject("Activity => Review_Disbursement_Setup => OnGetStageTransition")]
    internal class when_review_disbursement_setup : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Review_Disbursement_Setup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_review_disbursement_setup_stagetransition = () =>
        {
            result.ShouldEqual<string>("Review Disbursement Setup");
        };
    }
}