using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.EXT_ArchiveValuationClone.OnStart
{
    [Subject("Activity => EXT_ArchiveValuationClone => OnStart")] // AutoGenerated
    internal class when_ext_archivevaluationclone : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXT_ArchiveValuationClone(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}