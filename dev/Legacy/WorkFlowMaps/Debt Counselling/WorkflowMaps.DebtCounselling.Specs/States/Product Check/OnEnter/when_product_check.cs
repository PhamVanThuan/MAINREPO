using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Product_Check.OnEnter
{
    [Subject("State => Product_Check => OnEnter")] // AutoGenerated
    internal class when_product_check : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Product_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}