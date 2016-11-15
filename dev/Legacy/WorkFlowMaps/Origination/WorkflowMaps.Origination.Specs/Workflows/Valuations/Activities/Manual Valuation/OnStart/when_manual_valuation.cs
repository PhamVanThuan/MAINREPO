using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Manual_Valuation.OnStart
{
    [Subject("Activity => Manual_Valuation => OnStart")] // AutoGenerated
    internal class when_manual_valuation : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Manual_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}