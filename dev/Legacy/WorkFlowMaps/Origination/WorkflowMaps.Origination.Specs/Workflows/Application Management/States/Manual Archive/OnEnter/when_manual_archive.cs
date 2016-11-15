using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Manual_Archive.OnEnter
{
    [Subject("State => Manual_Archive => OnEnter")] // AutoGenerated
    internal class when_manual_archive : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manual_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}