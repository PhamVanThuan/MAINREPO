using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Pricing_Changes.OnStart
{
    [Subject("Activity => Pricing_Changes => OnStart")]
    internal class when_pricing_changes : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = "Approve with Pricing Changes";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Pricing_Changes(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}