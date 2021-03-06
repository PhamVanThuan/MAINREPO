using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Valuation_Return.OnEnter
{
    [Subject("State => Valuation_Return => OnEnter")] // AutoGenerated
    internal class when_valuation_return : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Valuation_Return(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}