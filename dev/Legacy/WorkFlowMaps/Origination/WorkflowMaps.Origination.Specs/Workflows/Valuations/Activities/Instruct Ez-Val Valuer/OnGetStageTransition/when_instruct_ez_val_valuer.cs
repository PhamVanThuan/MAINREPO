﻿using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Instruct_Ez_Val_Valuer.OnGetStageTransition
{
    [Subject("Activity => Instruct_Ez_Val_Valuer => OnGetStageTransition")]
    internal class when_instruct_ez_val_valuer : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Instruct_Ez_Val_Valuer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_instruct_ez_val_valuer_stagetransition = () =>
        {
            result.ShouldEqual<string>("Instruct EZ-Val Valuer");
        };
    }
}