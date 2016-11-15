using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Common_Counter_Proposal.OnEnter
{
    [Subject("State => Common_Counter_Proposal => OnEnter")] // AutoGenerated
    internal class when_common_counter_proposal : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Counter_Proposal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}