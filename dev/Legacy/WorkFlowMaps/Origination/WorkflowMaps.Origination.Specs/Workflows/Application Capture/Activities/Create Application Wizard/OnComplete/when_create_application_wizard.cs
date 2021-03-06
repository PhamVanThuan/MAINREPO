using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Application_Wizard.OnComplete
{
    [Subject("Activity => Create_Application_Wizard => OnComplete")] // AutoGenerated
    internal class when_create_application_wizard : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Create_Application_Wizard(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}