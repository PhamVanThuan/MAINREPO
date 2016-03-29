using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Instruction_Failed.OnComplete
{
    [Subject("Activity => Instruction_Failed => OnComplete")] // AutoGenerated
    internal class when_instruction_failed : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Instruction_Failed(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}