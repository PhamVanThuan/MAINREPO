using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Common_Manual_Valuation.OnEnter
{
    [Subject("State => Common_Manual_Valuation => OnEnter")] // AutoGenerated
    internal class when_common_manual_valuation : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Manual_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}