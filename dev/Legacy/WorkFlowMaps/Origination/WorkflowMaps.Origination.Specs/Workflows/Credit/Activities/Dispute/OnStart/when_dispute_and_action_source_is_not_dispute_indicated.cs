using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Dispute.OnStart
{
    [Subject("Activity => Dispute => OnStart")]
    internal class when_dispute_and_action_source_is_not_dispute_indicated : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = "Request Further Info";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Dispute(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}