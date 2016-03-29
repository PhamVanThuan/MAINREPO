using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Archive_Processor.OnReturn
{
    [Subject("State => Archive_Processor => OnReturn")] // AutoGenerated
    internal class when_archive_processor : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}