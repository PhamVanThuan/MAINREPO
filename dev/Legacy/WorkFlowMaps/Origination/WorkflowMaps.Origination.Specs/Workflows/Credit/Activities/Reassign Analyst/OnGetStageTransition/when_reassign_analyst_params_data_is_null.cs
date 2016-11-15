﻿using Machine.Specifications;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Reassign_Analyst.OnGetStageTransition
{
    [Subject("Activity => Reassign_Analyst => OnGetStageTransition")]
    internal class when_reassign_analyst_params_data_is_null : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = null;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reassign_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}