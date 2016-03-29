using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Continue_with_Application.OnGetStageTransition
{
    [Subject("Activity => Continue_with_Application => OnGetStageTransition")]
    internal class when_continue_with_application : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Continue with Application";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Continue_with_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_continue_with_application_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}