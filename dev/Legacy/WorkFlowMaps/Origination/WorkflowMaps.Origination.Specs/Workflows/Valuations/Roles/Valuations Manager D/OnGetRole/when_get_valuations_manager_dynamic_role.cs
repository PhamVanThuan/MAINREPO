using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Roles.Valuations_Manager_D.OnGetRole
{
    [Subject("Activity => Valuations_Manager_D => OnGetRole")]
    internal class when_get_valuations_manager_dynamic_role : WorkflowSpecValuations
    {
        private static IWorkflowAssignment client;
        private static string result;
        private static string expectedUser;

        private Establish context = () =>
        {
            result = String.Empty;
            client = An<IWorkflowAssignment>();
            expectedUser = "VMUser";
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            client.WhenToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Valuations Manager D", 0)).Return(expectedUser);
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_Valuations_Valuations_Manager_D(instanceData, workflowData, String.Empty, paramsData, messages);
        };

        private It should_call_resolve_valuation_manager_role = () =>
        {
            client.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, "Valuations Manager D", 0));
        };

        private It should_return_what_resolve_dynamic_role_returns_ = () =>
        {
            result.ShouldEqual<string>(expectedUser);
        };
    }
}