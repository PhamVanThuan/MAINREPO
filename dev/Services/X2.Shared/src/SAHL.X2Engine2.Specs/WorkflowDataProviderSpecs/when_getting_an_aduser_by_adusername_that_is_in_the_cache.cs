using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models._2AM;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_an_aduser_by_adusername_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static string adUserName = "sahl\\paulc";
        private static ADUserDataModel aduserDataModel;
        private static ADUserDataModel returnedModel;

        private Establish context = () =>
        {
            aduserDataModel = new ADUserDataModel(1, "userName", 1, "", "", "", 12345);
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, ADUserDataModel>(Param.IsAny<INamedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<ADUserDataModel>("Key")).Return(aduserDataModel);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetADUser(adUserName);
        };

        It should_get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<ADUserDataModel>("Key"));
            };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ADUserKey.ShouldEqual(1);
        };
    }
}