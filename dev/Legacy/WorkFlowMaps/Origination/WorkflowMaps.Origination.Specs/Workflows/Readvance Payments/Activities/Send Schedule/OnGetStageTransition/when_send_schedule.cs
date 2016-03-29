using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Send_Schedule.OnGetStageTransition
{
    internal class when_send_schedule : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_Schedule(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_readvance_payments_string = () =>
        {
            result.ShouldEqual("Send Schedule");
        };
    }
}