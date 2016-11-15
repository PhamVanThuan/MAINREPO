﻿using Machine.Specifications;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Refer_Senior_Analyst.OnGetStageTransition
{
    [Subject("Activity => Refer_Senior_Analyst => OnGetStageTransition")]
    internal class when_refer_senior_analyst : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = "efgh";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Refer_Senior_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_params_data_to_string = () =>
        {
            result.ShouldBeTheSameAs(paramsData.Data.ToString());
        };
    }
}