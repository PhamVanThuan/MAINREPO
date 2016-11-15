using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Archive_Valuation_Complete.OnReturn
{
    [Subject("State => Archive_Valuation_Complete => OnReturn")] // AutoGenerated
    internal class when_archive_valuation_complete : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Valuation_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}