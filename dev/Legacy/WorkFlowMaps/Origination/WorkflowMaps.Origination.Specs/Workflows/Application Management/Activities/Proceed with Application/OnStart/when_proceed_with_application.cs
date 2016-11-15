using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Proceed_with_Application.OnStart
{
    [Subject("Activity => Proceed_with_Application => OnStart")] // AutoGenerated
    internal class when_proceed_with_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Proceed_with_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}