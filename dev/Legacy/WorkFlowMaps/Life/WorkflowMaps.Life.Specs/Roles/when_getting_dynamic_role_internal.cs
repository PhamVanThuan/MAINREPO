using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Life.Specs.Roles
{
    internal class when_getting_dynamic_role_internal : WorkflowSpecLife
    {
        private static string result;
        private static string workflowName;
        private static string roleName;
        private static string consultant;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            client = An<IWorkflowAssignment>();
            consultant = @"SAHL\LCUser";
            roleName = "CurrentConsultant";
            client.WhenToldTo(x => x.GetConsultantForInstanceAndRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<string>())).Return(consultant);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            ((ParamsDataStub)paramsData).WorkflowName = "LifeOrigination";
        };

        private Because of = () =>
        {
            result = workflow.GetDynamicRoleInternal(instanceData, workflowData, roleName, paramsData, messages);
        };

        private It should_return_the_life_origination_current_consultant = () =>
        {
            result.ShouldBeTheSameAs(consultant);
        };
    }
}