using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.EXT_ArchiveFollowup.OnComplete
{
    [Subject("Activity => EXT_ArchiveFollowup => OnComplete")] // AutoGenerated
    internal class when_ext_archivefollowup : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_EXT_ArchiveFollowup(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}