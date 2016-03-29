using Machine.Specifications;
using System;

namespace WorkflowMaps.Valuations.Specs.Activities.Reassign_User.OnGetStageTransition
{
    [Subject("Activity => Reassign_User => OnGetStageTransition")]
    internal class when_reassign_user : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_reassign_user_stagetransition = () =>
        {
            result.ShouldEqual<string>("Reassign User");
        };
    }
}