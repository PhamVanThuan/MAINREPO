using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Followup.OnGetStageTransition
{
    [Subject("Activity => Create_Followup => OnGetStageTransition")]
    internal class when_create_followup : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Create Followup";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_create_followup_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}