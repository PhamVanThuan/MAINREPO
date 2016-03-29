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
    internal class when_archive_origination_and_is_fl_and_dont_archive_child_instances : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;
        private static string expectEWorkFolderID;
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
            expectedResult = true;
            workflowData.EWorkFolderID = "Test";
            expectEWorkFolderID = workflowData.EWorkFolderID;
            workflowData.IsFL = true;
            offerstatus = 3;
            offerinformationOriginal = -1;
            offerinformationAccepted = 3;
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

            appman.WhenToldTo(X => X.ArchiveChildInstances((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName)).Return(false);
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(expectedResult);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Origination(instanceData, workflowData, paramsData, messages);
        };

        private It should_complete_unhold_next_application = () =>
        {
            fl.WasToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_confirm_surety_signed = () =>
        {
            fl.WasToldTo(x => x.SuretySignedConfirmed((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_not_update_offer_status_original_offer = () =>
        {
            common.WasNotToldTo(x => x.UpdateOfferStatus(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_update_offer_status_accepted_offer = () =>
        {
            common.WasNotToldTo(x => x.UpdateOfferStatus(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_remove_detail_types = () =>
        {
            fl.WasNotToldTo(x => x.RemoveDetailTypes(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_not_deactivate_users_for_instance = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.DeActiveUsersForInstanceAndProcess(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<List<string>>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
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

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}