using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Dispute.OnStart
{
    [Subject("Activity => Dispute => OnStart")]
    internal class when_dispute : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = "Dispute Indicated";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Dispute(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}