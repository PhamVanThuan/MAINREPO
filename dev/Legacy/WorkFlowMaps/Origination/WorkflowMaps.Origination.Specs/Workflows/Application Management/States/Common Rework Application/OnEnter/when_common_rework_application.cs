using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Rework_Application.OnEnter
{
    [Subject("State => Common_Rework_Application => OnEnter")] // AutoGenerated
    internal class when_common_rework_application : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}