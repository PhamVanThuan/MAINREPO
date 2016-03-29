using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Reassign.OnExit
{
    [Subject("State => Common_Reassign => OnExit")] // AutoGenerated
    internal class when_common_reassign : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Reassign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}