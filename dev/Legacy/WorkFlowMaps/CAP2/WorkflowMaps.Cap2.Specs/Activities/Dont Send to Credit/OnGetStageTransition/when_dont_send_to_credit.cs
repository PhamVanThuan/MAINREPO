using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Dont_Send_to_Credit.OnGetStageTransition
{
    [Subject("Activity => Dont_Send_to_Credit => OnGetStageTransition")] // AutoGenerated
    internal class when_dont_send_to_credit : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Dont_Send_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}