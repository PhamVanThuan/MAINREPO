using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.FollowupReturn.OnExit
{
    [Subject("State => FollowupReturn => OnExit")] // AutoGenerated
    internal class when_followupreturn : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_FollowupReturn(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}