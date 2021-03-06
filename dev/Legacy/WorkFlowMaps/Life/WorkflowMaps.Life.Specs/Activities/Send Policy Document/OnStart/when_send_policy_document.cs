using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Send_Policy_Document.OnStart
{
    [Subject("Activity => Send_Policy_Document => OnStart")] // AutoGenerated
    internal class when_send_policy_document : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Send_Policy_Document(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}