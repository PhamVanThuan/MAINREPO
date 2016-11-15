using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Continue_Sale.OnStart
{
    [Subject("Activity => Continue_Sale => OnStart")] // AutoGenerated
    internal class when_continue_sale : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Continue_Sale(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}