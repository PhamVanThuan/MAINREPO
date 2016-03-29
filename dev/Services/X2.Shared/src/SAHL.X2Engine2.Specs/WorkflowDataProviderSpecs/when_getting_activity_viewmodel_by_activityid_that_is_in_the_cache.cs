using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_activity_viewmodel_by_activityid_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static Activity activity;
        private static Activity returnModel;

        Establish context = () =>
        {
            activity = new Activity(1, "activity", 1, "state1", 2, "state2", 1, false);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, Activity>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<Activity>("Key")).Return(activity);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetActivityById(1);
        };

        It should_get_it_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<Activity>("Key"));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ActivityID.ShouldEqual(1);
        };
    }
}