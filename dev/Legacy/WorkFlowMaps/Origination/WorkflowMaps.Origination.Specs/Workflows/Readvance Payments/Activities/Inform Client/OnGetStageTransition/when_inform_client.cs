using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Inform_Client.OnGetStageTransition
{
    [Subject("Activity => Inform_Client => OnGetStageTransition")]
    internal class when_inform_client : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Inform_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_Inform_Client = () =>
        {
            result.ShouldMatch("Inform Client");
        };
    }
}