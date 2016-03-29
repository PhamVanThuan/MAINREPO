using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Continue_with_Sale.OnGetStageTransition
{
    [Subject("Activity => Continue_with_Sale => OnGetStageTransition")] // AutoGenerated
    internal class when_continue_with_sale : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Continue_with_Sale(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}