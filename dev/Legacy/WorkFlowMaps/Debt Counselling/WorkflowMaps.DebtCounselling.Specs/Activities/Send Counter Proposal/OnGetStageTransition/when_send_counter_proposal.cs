using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Counter_Proposal.OnGetStageTransition
{
    [Subject("Activity => Send_Counter_Proposal => OnGetStageTransition")] // AutoGenerated
    internal class when_send_counter_proposal : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_Counter_Proposal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}