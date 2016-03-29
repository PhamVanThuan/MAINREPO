using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Archive_Lead.OnGetStageTransition
{
    [Subject("Activity => Archive_Lead => OnGetStageTransition")]
    internal class when_archive_lead : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Archive Lead";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Archive_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_archive_lead_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}