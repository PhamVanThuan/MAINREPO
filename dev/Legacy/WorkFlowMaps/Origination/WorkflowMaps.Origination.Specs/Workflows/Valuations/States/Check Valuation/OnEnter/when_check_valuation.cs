using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.States.Check_Valuation.OnEnter
{
    [Subject("State => Check_Valuation => OnEnter")] // AutoGenerated
    internal class when_check_valuation : WorkflowSpecValuations
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Check_Valuation(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}