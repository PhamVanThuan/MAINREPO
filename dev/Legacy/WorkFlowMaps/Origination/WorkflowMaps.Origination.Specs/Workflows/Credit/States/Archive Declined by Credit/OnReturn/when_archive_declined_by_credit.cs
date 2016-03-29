using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Archive_Declined_by_Credit.OnReturn
{
    [Subject("State => Archive_Declined_by_Credit => OnReturn")] // AutoGenerated
    internal class when_archive_declined_by_credit : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}