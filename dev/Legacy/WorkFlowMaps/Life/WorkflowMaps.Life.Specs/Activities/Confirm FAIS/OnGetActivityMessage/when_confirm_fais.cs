using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_FAIS.OnGetActivityMessage
{
    [Subject("Activity => Confirm_FAIS => OnGetActivityMessage")] // AutoGenerated
    internal class when_confirm_fais : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Confirm_FAIS(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}