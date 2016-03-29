using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_system_activities_for_state_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static List<ActivityDataModel> Activities;
        private static int stateId = 1;
        private static ActivityDataModel returnModel;

        private Establish context = () =>
            {
                Activities = new List<ActivityDataModel>();
                Activities.Add(new ActivityDataModel(1, 1, "name", 1, 1, 2, false, 1, 1, "activityMessage", null, null, null, "chainedActivityName", 1, Guid.NewGuid()));
                automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<ActivityDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
                automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
                automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ActivityDataModel>>("Key")).Return(Activities);
            };

        private Because of = () =>
            {
                returnModel = automocker.ClassUnderTest.GetSystemActivitiesForState(stateId).First();
            };

        private It should_return_system_activities_coming_from_that_state = () =>
            {
                returnModel.ID.ShouldEqual(1);
            };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ActivityDataModel>>("Key"));
        };
    }
}