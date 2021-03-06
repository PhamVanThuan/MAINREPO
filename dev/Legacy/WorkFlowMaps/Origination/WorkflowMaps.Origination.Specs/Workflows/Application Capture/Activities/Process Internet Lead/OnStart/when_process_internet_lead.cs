using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Process_Internet_Lead.OnStart
{
    [Subject("Activity => Process_Internet_Lead => OnStart")] // AutoGenerated
    internal class when_process_internet_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Process_Internet_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}