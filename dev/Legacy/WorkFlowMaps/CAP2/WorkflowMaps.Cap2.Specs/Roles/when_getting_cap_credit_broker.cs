﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Roles
{
    internal class when_getting_cap_credit_broker : WorkflowSpecCap2
    {
        private static string username;

        private Establish context = () =>
        {
            workflowData.CapCreditBroker = @"SAHL\ClintonS";
        };

        private Because of = () =>
        {
            username = workflow.OnGetRole_CAP2_Offers_CapCreditBroker(instanceData, workflowData, string.Empty, paramsData, messages);
        };

        private It should_return_the_cap_credit_broker_data_property = () =>
        {
            username.ShouldEqual(workflowData.CapCreditBroker);
        };
    }
}