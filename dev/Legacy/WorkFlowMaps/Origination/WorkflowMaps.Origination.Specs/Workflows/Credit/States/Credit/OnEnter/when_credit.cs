using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Credit.OnEnter
{
    [Subject("State => Credit => OnEnter")] // AutoGenerated
    internal class when_credit : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}