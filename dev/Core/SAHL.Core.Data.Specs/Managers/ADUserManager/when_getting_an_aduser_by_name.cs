using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using System.Collections.Generic;

namespace SAHL.Core.Data.Specs.Managers.ADUserManagerSpecs
{
    public class when_getting_an_aduser_by_name : WithFakes
    {
        private static IADUserDataManager adUserDataManager;
        private static ADUserManager adUserManager;
        private static string userName;
        private static IEnumerable<ADUserDataModel> adUserDataModelCollection;
        private static IEnumerable<ADUserDataModel> results;

        private Establish context = () =>
        {
            adUserDataModelCollection = new ADUserDataModel[] {
                new ADUserDataModel(1234, @"SAHL\Test", 1, "password", "passwordQuestion", "passwordAnswer", 1),
                new ADUserDataModel(12345, @"SAHL\Test2", 1, "password", "passwordQuestion", "passwordAnswer", 2)
            };
            userName = "userName";
            adUserDataManager = An<IADUserDataManager>();
            adUserManager = new ADUserManager(adUserDataManager);
            adUserDataManager.WhenToldTo(x => x.GetAdUserByUserName(userName)).Return(adUserDataModelCollection);
        };

        private Because of = () =>
        {
            results = adUserDataManager.GetAdUserByUserName(userName);
        };

        private It should_return_the_result_from_data_manager = () =>
        {
            results.ShouldEqual(adUserDataModelCollection);
        };
    }
}