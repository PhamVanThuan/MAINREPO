using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.EXT_CommonArchive.OnComplete
{
    [Subject("Activity => EXT_CommonArchive => OnComplete")] // AutoGenerated
    internal class when_ext_commonarchive : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_EXT_CommonArchive(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}