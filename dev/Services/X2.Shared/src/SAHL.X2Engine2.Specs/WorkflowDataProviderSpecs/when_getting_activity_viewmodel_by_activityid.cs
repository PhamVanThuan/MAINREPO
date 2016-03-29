using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_activity_viewmodel_by_activityid : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static Activity activity;
        private static Activity returnModel;

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            activity = new Activity(1, "activity", 1, "state1", 2, "state2", 1, false);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByIdSqlStatement>())).Return(activity);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetActivityById(1);
        };

        It should_add_it_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<Activity>(Param.IsAny<string>(), returnModel));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ActivityID.ShouldEqual(1);
        };
    }
}