using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Archive_Litigation.OnReturn
{
    [Subject("State => Archive_Litigation => OnReturn")] // AutoGenerated
    internal class when_archive_litigation : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Litigation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}