using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Policy_NTUd.OnReturn
{
    [Subject("State => Policy_NTUd => OnReturn")] // AutoGenerated
    internal class when_policy_ntud : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Policy_NTUd(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}