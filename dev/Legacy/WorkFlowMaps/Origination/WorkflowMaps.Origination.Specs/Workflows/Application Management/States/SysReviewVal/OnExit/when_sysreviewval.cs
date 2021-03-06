using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.SysReviewVal.OnExit
{
    [Subject("State => SysReviewVal => OnExit")] // AutoGenerated
    internal class when_sysreviewval : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_SysReviewVal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}