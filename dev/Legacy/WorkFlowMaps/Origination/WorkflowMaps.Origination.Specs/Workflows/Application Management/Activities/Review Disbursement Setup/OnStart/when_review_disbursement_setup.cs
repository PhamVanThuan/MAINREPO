using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Disbursement_Setup.OnStart
{
    [Subject("Activity => Review_Disbursement_Setup => OnStart")] // AutoGenerated
    internal class when_review_disbursement_setup : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Review_Disbursement_Setup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}