using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Folder_Archive.OnEnter
{
    [Subject("State => Folder_Archive => OnEnter")]
    internal class when_folder_archive : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static ICommon common;
        private static IFL fl;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", };
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Folder_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_offer_end_date = () =>
        {
            common.WasToldTo(x => x.SetOfferEndDate((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 3, 3));
        };

        private It should_remove_detail_types = () =>
        {
            fl.WasToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactivate_users_for_instance_and_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}