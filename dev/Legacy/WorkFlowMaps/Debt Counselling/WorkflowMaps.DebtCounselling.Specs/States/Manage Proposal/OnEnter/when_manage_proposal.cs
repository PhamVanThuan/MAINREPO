using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Manage_Proposal.OnEnter
{
    [Subject("State => Manage_Proposal => OnEnter")] // AutoGenerated
    internal class when_manage_proposal : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manage_Proposal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}