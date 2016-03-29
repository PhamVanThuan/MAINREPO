using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_AIP.OnEnter
{
    [Subject("State => Archive_AIP => OnEnter")]
    internal class when_archive_aip_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static int appkey;
        private static IFL fl;
        private static ICommon common;
        private static int offerstatus;

        private Establish context = () =>
        {
            result = false;
            offerstatus = 4;
            appkey = workflowData.ApplicationKey;
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_AIP(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_complete_unhold_next_application = () =>
        {
            fl.WasNotToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, appkey));
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, appkey, offerstatus, -1));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeTrue();
        };
    }
}