using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Roles.Branch_Admin_D.OnGetRole
{
    [Subject("Activity => Branch_Admin_D => OnGetRole")]
    internal class when_get_branch_admin_dynamic_role : WorkflowSpecApplicationCapture
    {
        private static string result;
        private static string expectedResult;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            client = An<IWorkflowAssignment>();
            expectedResult = "BAUser";
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            client.WhenToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD, 0)).Return(expectedResult); ;
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_Application_Capture_Branch_Admin_D(instanceData, workflowData, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD, paramsData, messages);
        };

        private It should_resolve_branch_admin_role = () =>
        {
            client.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD, 0));
        };

        private It should_return_what_resolve_dynamic_role_to_username_return = () =>
        {
            result.ShouldEqual<string>(expectedResult);
        };
    }
}