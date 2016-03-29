using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Return_to_sender.OnEnter
{
    [Subject("State => Return_to_sender => OnEnter")]
    internal class when_return_to_sender : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon client;

        private Establish context = () =>
        {
            result = false;
            client = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            workflowData.ApplicationKey = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Return_to_sender(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            client.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 1, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}