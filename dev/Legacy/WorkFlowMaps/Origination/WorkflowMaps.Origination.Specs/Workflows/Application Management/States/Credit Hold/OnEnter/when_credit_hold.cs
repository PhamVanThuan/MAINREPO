using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Credit_Hold.OnEnter
{
    [Subject("State => Credit_Hold => OnEnter")] // AutoGenerated
    internal class when_credit_hold : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Credit_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}