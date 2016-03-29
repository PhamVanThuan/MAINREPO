using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Folder_Archive.OnExit
{
    [Subject("State => Folder_Archive => OnExit")] // AutoGenerated
    internal class when_folder_archive : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Folder_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}