using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_Exp_LOA.OnEnter
{
    [Subject("State => Archive_Exp_LOA => OnEnter")]
    internal class when_archive_exp_LOA_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon common;
        private static IFL fl;
        private static int appkey;
        private static int offerstatus;

        private Establish context = () =>
        {
            workflowData.IsFL = true;
            result = false;
            appkey = workflowData.ApplicationKey;
            offerstatus = 4;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Exp_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, appkey, offerstatus, -1));
        };

        private It should_complete_unhold_next_application = () =>
        {
            fl.WasToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, appkey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}