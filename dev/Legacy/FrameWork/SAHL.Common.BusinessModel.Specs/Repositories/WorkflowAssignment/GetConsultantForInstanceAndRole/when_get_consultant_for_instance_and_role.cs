using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowAssignment.GetConsultantForInstanceAndRole
{
    internal class when_get_consultant_for_instance_and_role : WorkflowAssignmentRepositoryWithFakesBase
    {
        static long instanceID;
        static string consultant;
        static string dynamicRole;

        Establish context = () =>
        {
            instanceID = 1;
            consultant = @"SAHL\CraigF";
            dynamicRole = "Consultant";
            workflowAssignmentRepo.WhenToldTo(x => x.GetConsultantForInstanceAndRole(Param.IsAny<long>(), Param.IsAny<string>())).Return(consultant);
        };

        Because of = () =>
        {
            consultant = workflowAssignmentRepo.GetConsultantForInstanceAndRole(instanceID, dynamicRole);
        };

        It should_return_a_value = () =>
        {
            consultant.ShouldNotBeEmpty();
        };

        It should_return_a_value_containing_SAHL = () =>
        {
            consultant.ShouldContain("SAHL");
        };

        It should_call_GetConsultantForInstanceAndRole = () =>
        {
            workflowAssignmentRepo.WasToldTo(x => x.GetConsultantForInstanceAndRole(instanceID, dynamicRole));
        };
    }
}