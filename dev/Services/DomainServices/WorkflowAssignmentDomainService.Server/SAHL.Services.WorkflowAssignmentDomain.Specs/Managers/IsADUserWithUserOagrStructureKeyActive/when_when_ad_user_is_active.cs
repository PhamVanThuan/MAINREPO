using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_when_ad_user_is_active : WithFakes
    {
        private static int userOrgStructureKey;
        private static bool result;
        private static FakeDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
        private static ADUserDataModel expectedADUser;

        private Establish that = () =>
        {
            expectedADUser = new ADUserDataModel("Some user", 1, "password", null, null, 1001);
            userOrgStructureKey = 11001;
            dbFactory = An<FakeDbFactory>();

            dataManager = new WorkflowCaseDataManager(dbFactory);
            dbFactory.NewDb().InReadOnlyAppContext()
                .WhenToldTo(x => x.SelectWhere<ADUserDataModel>("[ADUserKey] = 11001", null)).Return(new List<ADUserDataModel> { expectedADUser });
        };

        private Because of = () =>
        {
            result = dataManager.IsADUserActive(userOrgStructureKey);
        };

        private It should_get_aduser = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.SelectWhere<ADUserDataModel>("[ADUserKey] = 11001", null));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}