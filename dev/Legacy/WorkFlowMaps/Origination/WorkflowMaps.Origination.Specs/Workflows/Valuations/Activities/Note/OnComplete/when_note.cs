using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Note.OnComplete
{
    [Subject("Activity => Note => OnComplete")] // AutoGenerated
    internal class when_note : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Note(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}