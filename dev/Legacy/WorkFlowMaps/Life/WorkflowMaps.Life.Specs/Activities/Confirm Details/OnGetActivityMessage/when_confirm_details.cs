﻿using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_Details.OnGetActivityMessage
{
    [Subject("Activity => Confirm_Details => OnGetActivityMessage")] // AutoGenerated
    internal class when_confirm_details : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Confirm_Details(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}