﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Recalculate_CAP2.OnGetActivityMessage
{
    [Subject("Activity => Recalculate_CAP2 => OnGetActivityMessage")]
    internal class when_recalculating_cap2 : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Recalculate_CAP2(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}