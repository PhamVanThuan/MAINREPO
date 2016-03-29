using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Close_Instance.OnComplete
{
    [Subject("Activity => Close_Instance => OnComplete")] // AutoGenerated
    internal class when_close_instance : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Close_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}