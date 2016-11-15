﻿using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Post_Cred_Appro.OnStart
{
    [Subject("State => Sys_Post_Cred_Appro => OnStart")]
    internal class when_sys_post_cred_appro_and_no_payments : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Sys_Post_Cred_Appro(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}