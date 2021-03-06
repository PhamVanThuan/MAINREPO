using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_Not_Accepted.OnEnter
{
    [Subject("State => Common_Not_Accepted => OnEnter")] // AutoGenerated
    internal class when_common_not_accepted : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}