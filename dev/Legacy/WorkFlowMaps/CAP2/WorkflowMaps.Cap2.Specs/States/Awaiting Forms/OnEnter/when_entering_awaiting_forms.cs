using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.States.Awaiting_Forms.OnEnter
{
    [Subject("State => Awaiting_Forms => OnEnter")]
    internal class when_entering_awaiting_forms : WorkflowSpecCap2
    {
        private static bool result;
        private static ICap2 client;

        private Establish context = () =>
        {
            result = false;
            client = An<ICap2>();
            domainServiceLoader.RegisterMockForType<ICap2>(client);
            workflowData.CapOfferKey = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Awaiting_Forms(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_update_the_offerstatus_to_awaiting_documents = () =>
        {
            client.WasToldTo(x => x.UpdateCapOfferStatus((IDomainMessageCollection)messages, workflowData.CapOfferKey, (int)SAHL.Common.Globals.CapStatuses.AwaitingDocuments));
        };
    }
}