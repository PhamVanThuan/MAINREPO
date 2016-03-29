using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_Decline.OnEnter
{
    [Subject("State => Archive_Decline => OnEnter")]
    internal class when_archive_decline_and_is_not_fl_and_archive_child_instances : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IFL fl;
        private static IWorkflowAssignment workFlowAssignment;
        private static ICommon common;
        private static int appkey;
        private static List<string> dynamicRoles;
        private static int offerstatus;
        private static string adUserName;
        private static IApplicationManagement applicationManagement;

        private Establish context = () =>
        {
            result = false;
            adUserName = string.Empty;
            appkey = workflowData.ApplicationKey;
            offerstatus = 5;
            dynamicRoles = new List<string>
            {
                "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D",
                "New Business Processor D",
                "New Business Manager D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D"
            };

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            workFlowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workFlowAssignment);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            applicationManagement = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagement);
            applicationManagement.WhenToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_complete_unhold_next_application = () =>
        {
            fl.WasNotToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_not_remove_detail_types = () =>
        {
            fl.WasNotToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_offerstatus = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, appkey, offerstatus, -1));
        };

        private It should_deactivate_users_for_instance_and_process = () =>
        {
            workFlowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, dynamicRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}