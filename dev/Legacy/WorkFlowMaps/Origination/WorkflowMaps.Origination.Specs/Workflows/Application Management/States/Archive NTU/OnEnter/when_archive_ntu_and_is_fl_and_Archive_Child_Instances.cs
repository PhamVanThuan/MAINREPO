﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_NTU.OnEnter
{
    [Subject("State => Archive_NTU => OnEnter")]
    internal class when_archive_ntu_and_is_fl_and_Archive_Child_Instances : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement applicationManagement;
        private static ICommon common;
        private static IFL fl;
        private static IWorkflowAssignment workflowAssignment;
        private static int offerstatus;
        private static List<string> workflowRoles;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            offerstatus = 4;
            applicationManagement = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagement);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);

            workflowRoles = new List<string>
            {
                "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D",
                "New Business Processor D",
                "New Business Manager D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D",
            };

            applicationManagement.WhenToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_complete_unhold_next_application = () =>
        {
            fl.WasToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_remove_detail_types = () =>
        {
            fl.WasToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_Not_Remove_Registration_Process_Detail_Types = () =>
        {
            applicationManagement.WasNotToldTo(x => x.RemoveRegistrationProcessDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_offerstatus = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, offerstatus, -1));
        };

        private It should_deactivate_the_users_for_this_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_update_accountstatus = () =>
        {
            common.WasNotToldTo(x => x.UpdateAccountStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 6));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}