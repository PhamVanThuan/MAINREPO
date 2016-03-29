using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resend_Instruction.OnGetStageTransition
{
    [Subject("Activity => Resend_Instruction => OnGetStageTransition")]
    internal class when_resend_instruction : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Resend_Instruction(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_resend_instruction = () =>
        {
            result.ShouldEqual("Resend Instruction");
        };
    }
}