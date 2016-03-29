using Machine.Specifications;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Further_Valuation_Requested.OnStart
{
    [Subject("Activity => Further_Valuation_Requested => OnStart")]
    internal class when_further_valuation_requested : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 3;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Valuation_Requested(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}