using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.States.Ready_to_Callback.OnEnter
{
    [Subject("State => Ready_to_Callback => OnEnter")]
    internal class when_entering_ready_to_callback : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;

        private Establish context = () =>
            {
                client = An<ILife>();
                result = false;
                domainServiceLoader.RegisterMockForType<ILife>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnEnter_Ready_to_Callback(instanceData, workflowData, paramsData, messages);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_perform_ready_to_callback_client_method = () =>
            {
                client.WasToldTo(x => x.ReadyToCallback(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>()));
            };
    }
}