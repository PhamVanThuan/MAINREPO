﻿using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Receive_Schedule.OnGetStageTransition
{
    [Subject("Activity => Receive_Schedule => OnGetStageTransition")] // AutoGenerated
    internal class when_receive_schedule : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Receive_Schedule(instanceData, workflowData, paramsData, messages);
        };

        private It should_receive_schedule_string = () =>
        {
            result.ShouldBeTheSameAs("Receive Schedule");
        };
    }
}