using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.New_Business.OnComplete
{
    [Subject("Activity => New_Business => OnComplete")] // AutoGenerated
    internal class when_new_business : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_New_Business(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}