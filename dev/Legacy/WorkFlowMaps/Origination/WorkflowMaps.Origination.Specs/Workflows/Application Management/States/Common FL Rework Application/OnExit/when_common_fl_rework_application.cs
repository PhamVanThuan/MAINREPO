using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_FL_Rework_Application.OnExit
{
    [Subject("State => Common_FL_Rework_Application => OnExit")] // AutoGenerated
    internal class when_common_fl_rework_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_FL_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}