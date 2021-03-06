using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Common_Court.OnEnter
{
    [Subject("State => Common_Court => OnEnter")] // AutoGenerated
    internal class when_common_court : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Court(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}