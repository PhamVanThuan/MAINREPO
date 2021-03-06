using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.FL_Rework_Application.OnStart
{
    [Subject("Activity => FL_Rework_Application => OnStart")] // AutoGenerated
    internal class when_fl_rework_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_FL_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}