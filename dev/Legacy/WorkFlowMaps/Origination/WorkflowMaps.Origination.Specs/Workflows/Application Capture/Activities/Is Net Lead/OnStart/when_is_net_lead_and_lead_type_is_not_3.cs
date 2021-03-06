﻿using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Net_Lead.OnStart
{
    [Subject("Activity => Is_Net_Lead => OnStart")] // AutoGenerated
    internal class when_is_net_lead_and_lead_type_is_not_3 : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Net_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}