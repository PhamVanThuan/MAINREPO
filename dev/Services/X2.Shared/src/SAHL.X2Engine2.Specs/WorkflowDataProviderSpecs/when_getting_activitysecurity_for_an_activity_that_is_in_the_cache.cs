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
    public class when_getting_activitysecurity_for_an_activity_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        static List<ActivitySecurityDataModel> items;
        private static ActivitySecurityDataModel activitySecurityDataModel;
        private static ActivitySecurityDataModel returnedModel;

        private Establish context = () =>
        {
            activitySecurityDataModel = new ActivitySecurityDataModel(10, 1, 1);
            items = new List<ActivitySecurityDataModel>(new ActivitySecurityDataModel[] { activitySecurityDataModel });
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<ActivitySecurityDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ActivitySecurityDataModel>>("Key")).Return(items);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetActivitySecurityForActivity(10).First();
        };

        It should_use_the_cache_to_return_the_item = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ActivitySecurityDataModel>>("Key"));
        };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ID.ShouldEqual(10);
        };
    }
}