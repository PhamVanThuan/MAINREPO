using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.States.Open_CAP2_Offer.OnEnter
{
    [Subject("State => Open_CAP2_Offer => OnEnter")]
    public class When_entering_open_cap2_offer : WorkflowSpecCap2
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
                workflow.OnEnter_Open_CAP2_Offer(instanceData, workflowData, paramsData, messages);
            };

        private It should_update_the_offerstatus_to_open = () =>
            {
                client.WasToldTo(x => x.UpdateCapOfferStatus((IDomainMessageCollection)messages, workflowData.CapOfferKey, (int)SAHL.Common.Globals.CapStatuses.Open));
            };
    }
}