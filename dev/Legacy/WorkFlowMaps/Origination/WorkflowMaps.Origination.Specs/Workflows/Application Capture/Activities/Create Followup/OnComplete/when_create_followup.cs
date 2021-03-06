using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Followup.OnComplete
{
    [Subject("Activity => Create_Followup => OnComplete")] // AutoGenerated
    internal class when_create_followup : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Create_Followup(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}