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
    public class when_getting_stage_activities_by_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static List<StageActivityDataModel> Activities;
        private static int activityId = 1;
        private static StageActivityDataModel returnModel;

        private Establish context = () =>
        {
            Activities = new List<StageActivityDataModel>();
            Activities.Add(new StageActivityDataModel(1, 1, 54, 568));
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<StageActivityDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<StageActivityDataModel>>("Key")).Return(Activities);
        };

        private Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetStageActivities(activityId).First();
        };

        It should_get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<StageActivityDataModel>>("Key"));
            };

        private It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}