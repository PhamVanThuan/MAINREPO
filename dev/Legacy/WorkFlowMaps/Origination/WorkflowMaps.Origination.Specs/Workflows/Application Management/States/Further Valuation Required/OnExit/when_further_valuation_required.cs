using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Further_Valuation_Required.OnExit
{
    [Subject("State => Further_Valuation_Required => OnExit")] // AutoGenerated
    internal class when_further_valuation_required : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Further_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}