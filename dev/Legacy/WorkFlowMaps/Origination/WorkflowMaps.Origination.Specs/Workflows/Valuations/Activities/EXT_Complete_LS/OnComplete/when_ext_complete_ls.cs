using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.EXT_Complete_LS.OnComplete
{
    [Subject("Activity => EXT_Complete_LS => OnComplete")] // AutoGenerated
    internal class when_ext_complete_ls : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_EXT_Complete_LS(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}