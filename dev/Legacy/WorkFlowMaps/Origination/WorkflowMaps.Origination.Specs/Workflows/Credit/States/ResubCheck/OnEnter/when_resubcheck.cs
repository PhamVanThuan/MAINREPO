using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.ResubCheck.OnEnter
{
    [Subject("State => ResubCheck => OnEnter")] // AutoGenerated
    internal class when_resubcheck : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_ResubCheck(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}