using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Retry_Instruction.OnGetStageTransition
{
    [Subject("Activity => Retry_Instruction => OnGetStageTransition")]
    internal class when_retry_instruction : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Retry_Instruction(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_retry_instruction_stagetransition = () =>
        {
            result.ShouldEqual<string>("Retry Instruction");
        };
    }
}