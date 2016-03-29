using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Dispute_Indicated.OnStart
{
    [Subject("Activity => Dispute_Indicated => OnStart")]
    internal class when_dispute_indicated : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Dispute_Indicated(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}