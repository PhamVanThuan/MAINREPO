using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_activity_by_workflow_and_name_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static Activity activity;
        private static Activity returnedModel;

        private Establish context = () =>
        {
            activity = new Activity(10, "activity", 10, "state1", 11, "state2", 1, false);
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, Activity>(Param.IsAny<INamedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<Activity>("Key")).Return(activity);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetActivityByNameAndWorkflowName("activity", "workflow");
        };

        It should_get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<Activity>("Key"));
            };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ActivityID.ShouldEqual(10);
        };
    }
}