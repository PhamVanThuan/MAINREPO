using Machine.Specifications;
using System;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Assign_EstateAgent_Lead.OnGetStageTransition
{
    [Subject("Activity => Assign_EstateAgent_Lead => OnGetStageTransition")]
    internal class when_assign_estateagent_lead : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Assign EstateAgent Lead";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Assign_EstateAgent_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_assign_estate_agent_lead_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}