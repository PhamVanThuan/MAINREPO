using Machine.Specifications;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Review_Valuation_Requested.OnStart
{
    [Subject("Activity => Review_Valuation_Requested => OnStart")]
    internal class when_review_valuation_required : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Review_Valuation_Requested(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}