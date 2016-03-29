using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.not_EA_Application.OnStart
{
    [Subject("Activity => not_EA_Application => OnStart")] // AutoGenerated
    internal class when_not_ea_application : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_not_EA_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}