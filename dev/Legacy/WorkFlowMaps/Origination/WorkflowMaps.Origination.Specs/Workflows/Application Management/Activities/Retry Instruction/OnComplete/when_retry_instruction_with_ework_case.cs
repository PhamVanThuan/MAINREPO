using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Retry_Instruction.OnComplete
{
    [Subject("Activity => Retry_Instruction => OnComplete")]
    internal class when_retry_instruction_with_ework_case : WorkflowSpecApplicationManagement
    {
        private static IApplicationManagement applicationManagementClient;
        private static ICommon commonClient;
        private static string expectedEworkFolderId;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            expectedEworkFolderId = "12345678901234";
            workflowData.EWorkFolderID = expectedEworkFolderId;

            applicationManagementClient = An<IApplicationManagement>();
            commonClient = An<ICommon>();

            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);

            commonClient.WhenToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, expectedEworkFolderId,
                         SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED, workflowData.ApplicationKey, paramsData.ADUserName, paramsData.StateName)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Retry_Instruction(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_add_instruction_sent_detail_type = () =>
        {
            applicationManagementClient.WasToldTo(x => x.AddDetailTypeInstructionSent((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_perform_x2_reinstructed_ework_action = () =>
        {
            commonClient.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, expectedEworkFolderId,
                            SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED, workflowData.ApplicationKey, paramsData.ADUserName, paramsData.StateName));
        };

        private It should_return_what_perform_ework_action_return = () =>
        {
            //mocked above to return true
            result.ShouldEqual<bool>(true);
        };
    }
}