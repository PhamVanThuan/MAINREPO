using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Return_to_sender.OnEnter
{
    [Subject("State => Return_To_Sender => OnEnter")]
    internal class when_return_to_sender : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Return_to_sender(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}