using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_Exp_LOA.OnExit
{
    [Subject("State => Archive_Exp_LOA => OnExit")] // AutoGenerated
    internal class when_archive_exp_loa : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Archive_Exp_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}