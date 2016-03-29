using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_an_activity_by_activating_external_activity_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ActivityDataModel results;
        private static ActivityDataModel activityModel;
        private static int externalActivityID;

        Establish context = () =>
        {
            externalActivityID = 999;
            activityModel = Helper.GetActivityDataModel();
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, ActivityDataModel>(Param.IsAny<INamedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<ActivityDataModel>("Key")).Return(activityModel);

            MockRepositoryProvider.GetReadWriteRepository();
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetActivityByActivatingExternalActivity(externalActivityID, Param.IsAny<int>());
        };

        It should_get_it_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<ActivityDataModel>("Key"));
        };

        It should_return_the_correct_one = () =>
        {
            results.ShouldBeTheSameAs(activityModel);
        };
    }
}