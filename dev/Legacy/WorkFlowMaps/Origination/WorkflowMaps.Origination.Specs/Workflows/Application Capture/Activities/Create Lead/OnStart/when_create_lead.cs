using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Lead.OnStart
{
    [Subject("Activity => Create_Lead => OnStart")] // AutoGenerated
    internal class when_create_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}