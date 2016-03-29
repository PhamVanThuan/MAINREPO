using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Send_LOA.OnGetStageTransition
{
    [Subject("Activity => Send_LOA => OnGetStageTransition")]
    internal class when_send_loa : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_send_loa_stagetransition = () =>
        {
            result.ShouldBeTheSameAs("Send LOA");
        };
    }
}