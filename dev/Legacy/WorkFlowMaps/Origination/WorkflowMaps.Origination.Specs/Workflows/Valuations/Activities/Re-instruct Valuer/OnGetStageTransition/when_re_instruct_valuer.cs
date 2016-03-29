using Machine.Specifications;
using System;

namespace WorkflowMaps.Valuations.Specs.Activities.Re_instruct_Valuer.OnGetStageTransition
{
    [Subject("Activity => Re_instruct_Valuer => OnGetStageTransition")]
    internal class when_re_instruct_valuer : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Re_instruct_Valuer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_re_instruct_valuer_stagetransition = () =>
        {
            result.ShouldEqual<string>("Re-instruct Valuer");
        };
    }
}