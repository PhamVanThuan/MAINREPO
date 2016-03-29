using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Application.OnGetStageTransition
{
    [Subject("Activity => Create_Application => OnGetStageTransition")]
    internal class when_create_application : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Create Application";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_create_application_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}