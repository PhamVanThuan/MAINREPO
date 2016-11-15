using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Application_Received.OnStart
{
    [Subject("Activity => Application_Received => OnStart")] // AutoGenerated
    internal class when_application_received : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Application_Received(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}