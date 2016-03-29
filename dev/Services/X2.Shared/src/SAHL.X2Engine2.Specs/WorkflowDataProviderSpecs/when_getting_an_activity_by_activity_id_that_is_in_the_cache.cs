using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_an_activity_by_activity_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ActivityDataModel results;
        private static ActivityDataModel activityModel;
        private static int activityID;

        Establish context = () =>
        {
            activityID = 999;
            activityModel = Helper.GetActivityDataModel();
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, ActivityDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<ActivityDataModel>("Key")).Return(activityModel);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetActivity(activityID);
        };

        It should_return_the_activity_data_model_provided_by_the_database = () =>
        {
            results.ShouldBeTheSameAs(activityModel);
        };

        It should_get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<ActivityDataModel>("Key"));
            };
    }
}