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
    internal class when_resend_instruction_where_data_prop_true_and_ework_folder_does_not_exists : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static int callCount;
        private static string expectedEFolderID;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            callCount = 0;
            string eFolderID;

            expectedEFolderID = "ExpectedEFolderID";

            workflowData.EWorkFolderID = string.Empty;
            ((ParamsDataStub)paramsData).Data = true;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appMan = An<IApplicationManagement>();
            appMan.Expect(x => x.CreateEWorkPipelineCase((IDomainMessageCollection)messages, workflowData.ApplicationKey, out eFolderID))
                .OutRef(expectedEFolderID)
                .Return(false)
                .IgnoreArguments()
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Resend_Instruction(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_resolve_branch_consultant_d_dynamic_role_to_user_name = () =>
        {
            assignment.WasNotToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()));
        };

        private It should_not_perform_x2_archive_ework_action = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_not_create_ework_pipeline_case = () =>
        {
            callCount.ShouldEqual(0);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}