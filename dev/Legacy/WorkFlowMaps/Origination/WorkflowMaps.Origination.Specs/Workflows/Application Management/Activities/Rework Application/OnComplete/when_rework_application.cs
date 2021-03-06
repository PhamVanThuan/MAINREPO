using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Rework_Application.OnComplete
{
    [Subject("Activity => Rework_Application => OnComplete")] // AutoGenerated
    internal class when_rework_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Rework_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}