using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Reassign.OnEnter
{
    [Subject("State => Common_Reassign => OnEnter")] // AutoGenerated
    internal class when_common_reassign : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Reassign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}