using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Decline.OnStart
{
    [Subject("Activity => Decline => OnStart")] // AutoGenerated
    internal class when_decline : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}