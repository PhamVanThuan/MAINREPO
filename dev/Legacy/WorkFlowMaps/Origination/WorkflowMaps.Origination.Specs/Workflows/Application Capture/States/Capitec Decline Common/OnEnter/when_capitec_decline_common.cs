using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Capitec_Decline_Common.OnEnter
{
    [Subject("State => Capitec_Decline_Common => OnEnter")] // AutoGenerated
    internal class when_capitec_decline_common : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Capitec_Decline_Common(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}