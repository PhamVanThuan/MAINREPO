using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Instruction_Failed.OnStart
{
    [Subject("Activity => Instruction_Failed => OnStart")] // AutoGenerated
    internal class when_instruction_failed : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Instruction_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}