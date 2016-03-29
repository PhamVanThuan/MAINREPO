using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Life.Specs.Roles
{
    internal class when_getting_life_origination_current_consulant : WorkflowSpecLife
    {
        private static IWorkflowAssignment client;
        private static string lifeConsultant;
        private static string result;

        private Establish context = () =>
        {
            lifeConsultant = @"SAHL\LCUser";
            client = An<IWorkflowAssignment>();
            client.WhenToldTo(x => x.GetConsultantForInstanceAndRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<string>())).Return(lifeConsultant);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_LifeOrigination_CurrentConsultant(instanceData, workflowData, string.Empty, paramsData, messages);
        };

        private It should_return_the_consultant_for_instance_and_role = () =>
        {
            result.ShouldEqual(lifeConsultant);
        };
    }
}