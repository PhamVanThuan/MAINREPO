using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Escalate_to_Manager.OnGetStageTransition
{
    [Subject("Activity => Escalate_to_Manager => OnGetStageTransition")]
    internal class when_escalate_to_manager : WorkflowSpecValuations
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            expectedResult = "Escalate to Manager";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Escalate_to_Manager(instanceData, workflowData, paramsData, messages);
        };

        private It should_retrun = () =>
        {
            result.ShouldEqual<string>(expectedResult);
        };
    }
}