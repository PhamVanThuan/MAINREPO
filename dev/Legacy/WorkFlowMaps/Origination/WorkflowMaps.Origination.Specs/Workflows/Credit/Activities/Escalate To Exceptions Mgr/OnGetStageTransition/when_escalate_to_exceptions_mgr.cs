﻿using Machine.Specifications;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Escalate_To_Exceptions_Mgr.OnGetStageTransition
{
    [Subject("Activity => Escalate_To_Exceptions_Mgr => OnGetStageTransition")]
    internal class when_escalate_to_exceptions_mgr : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = "efgh";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Escalate_To_Exceptions_Mgr(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_params_data_to_string = () =>
        {
            result.ShouldBeTheSameAs(paramsData.Data.ToString());
        };
    }
}