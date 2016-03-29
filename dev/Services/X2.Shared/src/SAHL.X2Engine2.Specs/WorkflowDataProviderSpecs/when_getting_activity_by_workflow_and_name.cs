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
    public class when_getting_activity_by_workflow_and_name : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static Activity activity;
        private static Activity returnedModel;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            activity = new Activity(10, "activity", 10, "state1", 11, "state2", 1, false);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByNameAndWorkflowNameSqlStatement>())).Return(activity);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetActivityByNameAndWorkflowName("activity", "workflow");
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<Activity>(Param.IsAny<string>(), activity));
            };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ActivityID.ShouldEqual(10);
        };
    }
}