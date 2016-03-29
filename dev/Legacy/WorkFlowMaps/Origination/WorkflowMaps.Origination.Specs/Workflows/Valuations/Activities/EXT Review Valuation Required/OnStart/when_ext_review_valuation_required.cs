using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.EXT_Review_Valuation_Required.OnStart
{
    [Subject("Activity => EXT_Review_Valuation_Required => OnStart")] // AutoGenerated
    internal class when_ext_review_valuation_required : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXT_Review_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}