using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_PipeLine.OnComplete
{
    [Subject("Activity => NTU_PipeLine => OnComplete")]
    internal class when_ntu_pipeline_where_is_not_fl_and_no_ework_folder_id : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;
        private static IFL fl;
        private static string expectedAssignedTo;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedAssignedTo = "AssignedToTest";
            workflowData.IsFL = false;
            workflowData.EWorkFolderID = string.Empty;
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU_PipeLine(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_perform_initial_fl_ntu = () =>
        {
            fl.WasNotToldTo(x => x.InitialFLNTU((IDomainMessageCollection)messages, paramsData.ADUserName, workflowData.ApplicationKey, instanceData.ID));
        };

        private It should_update_offer_status_to_NTU = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
        };

        private It should_send_email_to_consultant_for_query = () =>
        {
            appMan.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                instanceData.ID,
                (int)SAHL.Common.Globals.ReasonTypeGroups.NTU));
        };

        private It should_not_resolve_dynamic_role_to_user_name = () =>
        {
            assignment.WasNotToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Branch Consultant D", instanceData.ID));
        };

        private It should_not_perform_ework_action_x2_ntu_advise = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2NTUAdvise,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
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