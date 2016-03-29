using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Roles.Credit_Exceptions_D.OnGetCredit
{
    [Subject("Roles => Credit_Exceptions_D => OnGetCredit")]
    internal class when_credit_exceptions_d : WorkflowSpecCredit
    {
        private static string result;
        private static string expectedResult;
        private static string roleName;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            result = string.Empty;
            expectedResult = "Test";
            roleName = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;
            client = An<IWorkflowAssignment>();
            client.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<long>()))
                .Return(expectedResult); ;
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_Credit_Credit_Exceptions_D(instanceData, workflowData, roleName, paramsData, messages);
        };

        private It should_resolve_credit_exceptions_d_role_to_user_name = () =>
        {
            client.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Credit Exceptions D", instanceData.ID));
        };

        private It should_return_user_name = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}