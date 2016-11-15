using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Archive_Approve_Resub.OnExit
{
    [Subject("State => Archive_Approve_Resub => OnExit")] // AutoGenerated
    internal class when_archive_approve_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Approve_Resub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}