using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.States.Callback_Hold.OnEnter
{
    [Subject("State => Callback_Hold => OnEnter")]
    internal class when_entering_callback_hold : WorkflowSpecCap2
    {
        private static ICap2 client;

        private Establish context = () =>
        {
            client = An<ICap2>();
            domainServiceLoader.RegisterMockForType<ICap2>(client);
            workflowData.CapOfferKey = 1;
        };

        private Because of = () =>
        {
            workflow.OnEnter_Callback_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_offerstatus_to_callback_hold = () =>
        {
            client.WasToldTo(x => x.UpdateCapOfferStatus((IDomainMessageCollection)messages, workflowData.CapOfferKey, (int)SAHL.Common.Globals.CapStatuses.CallbackHold));
        };
    }
}