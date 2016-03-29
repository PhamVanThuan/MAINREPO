using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Send_AIP.OnGetStageTransition
{
    [Subject("Activity => Send_AIP => OnGetStageTransition")]
    internal class when_send_aip : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_AIP(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_send_aip_stagetransition = () =>
        {
            result.ShouldBeTheSameAs("Send AIP");
        };
    }
}