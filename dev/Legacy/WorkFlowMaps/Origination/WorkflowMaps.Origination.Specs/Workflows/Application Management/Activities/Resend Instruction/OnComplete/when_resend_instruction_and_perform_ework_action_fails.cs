using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resend_Instruction.OnComplete
{
    [Subject("Activity => Resend_Instruction => OnComplete")]
    internal class when_resend_instruction_and_perform_ework_action_fails : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static int callCount;
        private static string originalEFolderID;
        private static string expectedAssignedTo;
        private static string expectedEFolderID;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            callCount = 0;
            string eFolderID;

            originalEFolderID = "HasEFolderID";
            expectedAssignedTo = "ExpectedAssignedTo";
            expectedEFolderID = "ExpectedEFolderID";
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;
            ((ParamsDataStub)paramsData).ADUserName = "ExpectedADUserName";
            ((ParamsDataStub)paramsData).StateName = "ExpectedStateName";

            workflowData.EWorkFolderID = originalEFolderID;
            ((ParamsDataStub)paramsData).Data = true;

            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedAssignedTo);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appMan = An<IApplicationManagement>();
            appMan.Expect(x => x.CreateEWorkPipelineCase((IDomainMessageCollection)messages, workflowData.ApplicationKey, out eFolderID))
                .OutRef(expectedEFolderID)
                .Return(true)
                .IgnoreArguments()
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Resend_Instruction(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_resolve_branch_consultant_d_dynamic_role_to_user_name = () =>
        {
            assignment.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Branch Consultant D", instanceData.ID));
        };

        private It should_perform_x2_archive_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                originalEFolderID,
                SAHL.Common.Constants.EworkActionNames.X2ARCHIVE,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_not_create_ework_pipeline_case = () =>
        {
            callCount.ShouldEqual(0);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}