using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_Quote.OnGetStageTransition
{
    [Subject("Activity => Accept_Quote => OnGetStageTransition")] // AutoGenerated
    internal class when_accept_quote : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Accept_Quote(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}