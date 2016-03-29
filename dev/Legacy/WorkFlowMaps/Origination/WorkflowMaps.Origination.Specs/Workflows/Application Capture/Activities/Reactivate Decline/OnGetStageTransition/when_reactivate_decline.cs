﻿using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Reactivate_Decline.OnGetStageTransition
{
    [Subject("Activity => Reactivate_Decline => OnGetStageTransition")]
    internal class when_reactivate_decline : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reactivate_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_reactivate_decline = () =>
        {
            result.ShouldMatch("Reactivate Decline");
        };
    }
}