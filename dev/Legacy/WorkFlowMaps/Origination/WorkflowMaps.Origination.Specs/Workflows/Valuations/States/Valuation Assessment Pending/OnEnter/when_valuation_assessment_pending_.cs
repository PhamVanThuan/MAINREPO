using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Valuation_Assessment_Pending_.OnEnter
{
    [Subject("State => Valuation_Assessment_Pending_ => OnEnter")] // AutoGenerated
    internal class when_valuation_assessment_pending_ : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Valuation_Assessment_Pending_(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}