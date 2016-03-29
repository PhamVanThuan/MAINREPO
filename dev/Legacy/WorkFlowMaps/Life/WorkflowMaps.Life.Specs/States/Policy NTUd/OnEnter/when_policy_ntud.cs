using Machine.Specifications;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.States.Policy_NTUd.OnEnter
{
    [Subject("State => Policy_NTUd => OnEnter")]
    internal class when_policy_ntud : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Policy_NTUd(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}