using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_retrieving_last_user_to_act_on_a_case_with_capability : WithFakes
    {
        private static IDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
        private static int instanceId, capabilityKey;
        private static UserModel result;
        private static UserModel expectedUser;

        private Establish that = () =>
        {
            expectedUser = new UserModel("Some userName", 110, "Some user");
            dbFactory = An<IDbFactory>();
            dataManager = new WorkflowCaseDataManager(dbFactory);

            instanceId = 1;
            capabilityKey = 110;
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WhenToldTo(a => a.Select(Param<GetLastAssignedUserByCapabilityForInstanceStatement>
                    .Matches(b => b.CapabilityKey == capabilityKey && b.InstanceId == instanceId))).Return(new List<UserModel> { expectedUser });
        };

        private Because of = () =>
        {
            result = dataManager.GetLastUserAssignedInCapability(capabilityKey, instanceId);
        };

        private It should_get_the_last_user = () =>
        {
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(a => a.Select(Param<GetLastAssignedUserByCapabilityForInstanceStatement>
                    .Matches(b => b.CapabilityKey == capabilityKey && b.InstanceId == instanceId)))
                .OnlyOnce();
        };

        private It should_return_user = () =>
        {
            result.Equals(expectedUser).ShouldBeTrue();
        };
    }
}