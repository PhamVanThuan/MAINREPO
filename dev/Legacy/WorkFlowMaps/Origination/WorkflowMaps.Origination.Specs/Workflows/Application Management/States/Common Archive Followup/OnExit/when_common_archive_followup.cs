using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Archive_Followup.OnExit
{
    [Subject("State => Common_Archive_Followup => OnExit")] // AutoGenerated
    internal class when_common_archive_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Archive_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}