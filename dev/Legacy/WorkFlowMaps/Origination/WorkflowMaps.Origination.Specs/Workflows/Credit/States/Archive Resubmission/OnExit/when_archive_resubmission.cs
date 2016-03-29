using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Archive_Resubmission.OnExit
{
    [Subject("State => Archive_Resubmission => OnExit")] // AutoGenerated
    internal class when_archive_resubmission : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Resubmission(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}