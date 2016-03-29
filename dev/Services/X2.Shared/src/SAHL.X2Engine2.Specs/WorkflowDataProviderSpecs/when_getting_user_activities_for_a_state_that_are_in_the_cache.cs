using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_user_activities_for_a_state_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ActivityDataModel activityDataModel1;
        private static ActivityDataModel activityDataModel2;
        private static IEnumerable<ActivityDataModel> activities;
        private static IEnumerable<ActivityDataModel> results;
        private static int stateID;

        Establish context = () =>
        {
            stateID = 999;
            activityDataModel1 = Helper.GetActivityDataModel();
            activityDataModel2 = Helper.GetActivityDataModel();
            activityDataModel2.Name = "Second Activity";
            activityDataModel2.ID = 99;
            activities = new[] { activityDataModel1, activityDataModel2 };
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<ActivityDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ActivityDataModel>>("Key")).Return(activities);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetUserActivitiesForState(stateID);
        };

        It should_return_a_list_of_activity_data_models_for_the_state_provided = () =>
        {
            results.ShouldBeTheSameAs(activities);
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ActivityDataModel>>("Key"));
        };
    }
}