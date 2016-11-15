using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Note.OnGetActivityMessage
{
    [Subject("Activity => Note => OnGetActivityMessage")] // AutoGenerated
    internal class when_note : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Note(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}