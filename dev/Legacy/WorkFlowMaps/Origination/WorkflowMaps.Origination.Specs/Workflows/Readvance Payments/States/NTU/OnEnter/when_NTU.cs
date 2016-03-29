using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.NTU.OnEnter
{
    [Subject("State => NTU => OnEnter")]
    internal class when_NTU : WorkflowSpecReadvancePayments
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
            result = workflow.OnEnter_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}