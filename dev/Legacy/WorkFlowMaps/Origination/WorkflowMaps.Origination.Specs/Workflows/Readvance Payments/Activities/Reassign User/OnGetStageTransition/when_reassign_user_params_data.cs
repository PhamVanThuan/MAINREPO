﻿using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reassign_User.OnGetStageTransition
{
    [Subject("Activity => Reassign_User => OnGetStageTransition")]
    internal class when_reassign_user_params_data : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
            ((ParamsDataStub)paramsData).Data = "TestData";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldMatch("TestData");
        };
    }
}