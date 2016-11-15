using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Decline.OnEnter
{
    [Subject("State => Common_Decline => OnEnter")] // AutoGenerated
    internal class when_common_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}