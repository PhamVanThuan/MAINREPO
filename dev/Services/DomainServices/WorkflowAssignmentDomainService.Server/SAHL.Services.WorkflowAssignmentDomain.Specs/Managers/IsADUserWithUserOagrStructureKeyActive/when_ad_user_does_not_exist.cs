using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_ad_user_does_not_exist : WithFakes
    {
        private static int userOrgStructureKey;
        private static bool result;
        private static FakeDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;

        private Establish that = () =>
        {
            userOrgStructureKey = 11001;
            dbFactory = An<FakeDbFactory>();

            dataManager = new WorkflowCaseDataManager(dbFactory);
            dbFactory.NewDb().InReadOnlyAppContext()
                .WhenToldTo(x => x.SelectWhere<ADUserDataModel>("[ADUserKey] = 11001", null)).Return(new List<ADUserDataModel> { });
        };

        private Because of = () =>
        {
            result = dataManager.IsADUserActive(userOrgStructureKey);
        };

        private It should_get_aduser = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.SelectWhere<ADUserDataModel>("[ADUserKey] = 11001", null));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}