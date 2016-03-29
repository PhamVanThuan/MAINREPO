using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Data.Models._2AM.Managers.ADUser.Statements;
using SAHL.Core.Testing.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Data.Specs.Managers.ADUser
{
    public class when_getting_an_aduser_by_name : WithFakes
    {
        private static ADUserDataManager adUserDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static IEnumerable<ADUserDataModel> adUserCollection;
        private static IEnumerable<ADUserDataModel> results;
        private static string adUserName;

        private Establish context = () =>
            {
                adUserName = "SAHL\\ClintonS";
                adUserCollection = new ADUserDataModel[] { new ADUserDataModel("SAHL\\ClintonS", 1, "test", "test", "test", 1234) };
                fakeDbFactory = new FakeDbFactory();
                adUserDataManager = new ADUserDataManager(fakeDbFactory);
                fakeDbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select(Param.IsAny<GetAdUserByUserNameStatement>())).Return(adUserCollection);
            };

        private Because of = () =>
            {
                results = adUserDataManager.GetAdUserByUserName(adUserName);
            };

        private It should_return_the_results_from_the_query = () =>
            {
                results.First().ADUserName.ShouldEqual(@"SAHL\ClintonS");
            };
    }
}