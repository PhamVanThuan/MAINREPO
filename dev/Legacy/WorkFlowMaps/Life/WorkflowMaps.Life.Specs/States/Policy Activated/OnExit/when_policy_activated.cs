using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Policy_Activated.OnExit
{
    [Subject("State => Policy_Activated => OnExit")] // AutoGenerated
    internal class when_policy_activated : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Policy_Activated(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}