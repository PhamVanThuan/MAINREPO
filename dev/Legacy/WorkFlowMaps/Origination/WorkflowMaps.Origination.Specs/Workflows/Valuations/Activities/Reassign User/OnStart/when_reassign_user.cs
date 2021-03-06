using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Reassign_User.OnStart
{
    [Subject("Activity => Reassign_User => OnStart")] // AutoGenerated
    internal class when_reassign_user : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}