using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.OnFollowup.OnStart
{
    [Subject("Activity => OnFollowup => OnStart")] // AutoGenerated
    internal class when_onfollowup : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_OnFollowup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}