using Machine.Specifications;
using System;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Continue_Application.OnGetStageTransition
{
    [Subject("Activity => Continue_Application => OnGetStageTransition")]
    internal class when_continue_application : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Continue Application";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Continue_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_continue_application_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}