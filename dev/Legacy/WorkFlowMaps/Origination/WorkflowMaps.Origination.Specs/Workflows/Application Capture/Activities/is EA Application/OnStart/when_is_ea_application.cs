using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.is_EA_Application.OnStart
{
    [Subject("Activity => is_EA_Application => OnStart")] // AutoGenerated
    internal class when_is_ea_application : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.isEstateAgentApplication = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_is_EA_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}