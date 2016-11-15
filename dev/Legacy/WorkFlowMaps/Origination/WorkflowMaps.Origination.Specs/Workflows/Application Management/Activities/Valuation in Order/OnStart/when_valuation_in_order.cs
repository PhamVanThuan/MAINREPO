using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Valuation_in_Order.OnStart
{
    [Subject("Activity => Valuation_in_Order => OnStart")] // AutoGenerated
    internal class when_valuation_in_order : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Valuation_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}