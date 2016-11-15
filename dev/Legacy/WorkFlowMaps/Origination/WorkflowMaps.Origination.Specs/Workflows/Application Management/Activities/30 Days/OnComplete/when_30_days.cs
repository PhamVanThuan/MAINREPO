using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities._30_Days.OnComplete
{
    [Subject("Activity => 30_Days => OnComplete")] // AutoGenerated
    internal class when_30_days : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_30_Days(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}