using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Credit_Create.OnExit
{
    [Subject("State => Credit_Create => OnExit")] // AutoGenerated
    internal class when_credit_create : WorkflowSpecCredit
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnExit_Credit_Create(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}