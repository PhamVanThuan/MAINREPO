using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Retry_Instruction.OnGetActivityMessage
{
    [Subject("Activity => Retry_Instruction => OnGetActivityMessage")] // AutoGenerated
    internal class when_retry_instruction : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Retry_Instruction(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}