using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Capitec_Branch_Decline.OnEnter
{
    [Subject("State => Capitec_Branch_Decline => OnEnter")] // AutoGenerated
    internal class when_capitec_branch_decline : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Capitec_Branch_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}