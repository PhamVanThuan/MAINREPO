using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Update_Followup.OnComplete
{
    [Subject("Activity => Update_Followup => OnComplete")] // AutoGenerated
    internal class when_update_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Update_Followup(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}