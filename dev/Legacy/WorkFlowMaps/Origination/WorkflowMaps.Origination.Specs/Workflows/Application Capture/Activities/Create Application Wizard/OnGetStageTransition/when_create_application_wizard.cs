using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Application_Wizard.OnGetStageTransition
{
    [Subject("Activity => Create_Application_Wizard => OnGetStageTransition")]
    internal class when_create_application_wizard : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Create Application Wizard";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Application_Wizard(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_create_application_wizard_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}