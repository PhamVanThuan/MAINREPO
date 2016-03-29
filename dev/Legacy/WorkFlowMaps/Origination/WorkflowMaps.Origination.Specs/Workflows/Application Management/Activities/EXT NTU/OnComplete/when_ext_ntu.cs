using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.EXT_NTU.OnComplete
{
    [Subject("Activity => EXT_NTU => OnComplete")]
    internal class when_ext_ntu : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_update_offer_status_to_ntu = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
        };

        private It should_send_email_to_consultant_for_query = () =>
        {
            appMan.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID, (int)SAHL.Common.Globals.ReasonTypeGroups.NTU));
        };

        private It should_ntu_case = () =>
        {
            appMan.WasToldTo(x => x.NTUCase((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}