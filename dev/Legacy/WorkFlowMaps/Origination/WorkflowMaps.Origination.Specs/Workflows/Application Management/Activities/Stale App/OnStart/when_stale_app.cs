using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Stale_App.OnStart
{
    [Subject("Activity => Stale_App => OnStart")] // AutoGenerated
    internal class when_stale_app : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Stale_App(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}