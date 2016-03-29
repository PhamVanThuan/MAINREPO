using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Assign_EstateAgent.OnGetStageTransition
{
    [Subject("Activity => Assign_EstateAgent => OnGetStageTransition")]
    internal class when_assign_estateagent : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Assign EstateAgent";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Assign_EstateAgent(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_assign_estate_agent_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}