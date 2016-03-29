using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_Origination.OnEnter
{
    [Subject("State => Archive_Origination => OnEnter")]
    internal class when_archive_origination_where_is_not_fl_dont_archive_child_instance_and_efid_empty : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IFL fl;
        private static ICommon common;
        private static IApplicationManagement appman;
        private static int offerstatus;
        private static int offerinformationOriginal;
        private static int offerinformationAccepted;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment workflowAssignment;
        private static string dynamicRole;
        private static string expectedStateName;

        private Establish context = () =>
        {
            result = true;
            workflowData.IsFL = false;
            workflowData.EWorkFolderID = string.Empty;
            offerstatus = (int)SAHL.Common.Globals.OfferStatuses.Accepted;
            offerinformationOriginal = -1;
            offerinformationAccepted = (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer;
            dynamicRole = "Branch Consultant D";
            expectedStateName = SAHL.Common.Constants.EworkActionNames.X2DISBURSEMENTTIMER;
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            appman = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appman);
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

            appman.WhenToldTo(X => X.ArchiveChildInstances(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<string>()))
                .Return(true);
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Origination(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_complete_unhold_next_application = () =>
        {
            fl.WasNotToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_not_confirm_surety_signed = () =>
        {
            fl.WasNotToldTo(x => x.SuretySignedConfirmed(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_archive_application_management_children = () =>
        {
            appman.WasToldTo(x => x.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName));
        };

        private It should_remove_detail_types = () =>
        {
            fl.WasToldTo(x => x.RemoveDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactivate_users_for_instance_and_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_resolve_branch_consultant_dynamic_role = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<long>()));
        };

        private It should_not_perform_ework_action = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}