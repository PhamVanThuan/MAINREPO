using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Application_Not_Accepted.OnGetStageTransition
{
    [Subject("Activity => Application_Not_Accepted => OnGetStageTransition")]
    internal class when_application_not_accepted : WorkflowSpecApplicationCapture
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            result = String.Empty;
            expectedResult = "Application Not Accepted";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Application_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_application_not_accepted_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}