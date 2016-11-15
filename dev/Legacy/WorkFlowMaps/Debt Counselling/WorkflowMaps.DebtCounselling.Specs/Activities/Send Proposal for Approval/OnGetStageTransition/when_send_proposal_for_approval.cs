using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Proposal_for_Approval.OnGetStageTransition
{
    [Subject("Activity => Send_Proposal_for_Approval => OnGetStageTransition")] // AutoGenerated
    internal class when_send_proposal_for_approval : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_Proposal_for_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}