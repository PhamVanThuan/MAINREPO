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
    public class when_getting_a_workflowactivity_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static WorkflowActivity workflowActivity;
        private static WorkflowActivity returnModel;

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            workflowActivity = new WorkflowActivity(1, "wf1", "wf2", 2, 1, "activity");
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<WorkflowActivity>(Param.IsAny<WorkflowActivityByIdSqlStatement>())).Return(workflowActivity);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowActivityById(1);
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<WorkflowActivity>(Param.IsAny<string>(), returnModel));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}