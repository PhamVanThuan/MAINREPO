using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Credit_Decision_Check.OnEnter
{
    [Subject("State => Credit_Decision_Check => OnEnter")] // AutoGenerated
    internal class when_credit_decision_check : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Credit_Decision_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}