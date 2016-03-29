using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Continue_Sale.OnGetStageTransition
{
    [Subject("Activity => Continue_Sale => OnGetStageTransition")] // AutoGenerated
    internal class when_continue_sale : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Continue_Sale(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}