using Machine.Specifications;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_Requested.OnStart
{
    [Subject("Activity => Valuation_Requested => OnStart")]
    internal class when_not_valuation_request : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.EntryPath = 233;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Valuation_Requested(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}