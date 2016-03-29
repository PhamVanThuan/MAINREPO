using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.App_Not_Accepted_Finalised.OnGetStageTransition
{
    [Subject("Activity => App_Not_Accepted_Finalised => OnGetStageTransition")]
    internal class when_app_not_accepted_finalised : WorkflowSpecApplicationCapture
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            expectedResult = "App Not Accepted Finalised";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_App_Not_Accepted_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_app_not_accepted_finalised_stage_transition = () =>
        {
            result.ShouldEqual<string>(expectedResult);
        };
    }
}