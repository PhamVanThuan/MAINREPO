using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_an_aduser_by_adusername : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static string adUserName = "sahl\\paulc";
        private static ADUserDataModel aduserDataModel;
        private static ADUserDataModel returnedModel;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            aduserDataModel = new ADUserDataModel(1, "userName", 1, "", "", "", 12345);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<ADUserDataModel>(Param.IsAny<ADUserByAdUserNameSqlStatement>())).Return(aduserDataModel);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetADUser(adUserName);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<ADUserDataModel>(Param.IsAny<string>(), aduserDataModel));
            };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ADUserKey.ShouldEqual(1);
        };
    }
}